using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Application.Interfaces.Repositories;
using UniSozluk.Common.Events.User;
using UniSozluk.Common.Infrastructure;
using UniSozluk.Common;
using UniSozluk.Common.Infrastructure.Exceptions;
using UniSozluk.Common.Models.RequestModels;

namespace UniSozluk.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }
        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser =await userRepository.GetByIdAsync(request.UserId);

            if (dbUser is null)
                throw new DatabaseValidationException("Useral not found!");

            var dbEmailAddress = dbUser.EmailAdress;
            var emailChanged=string.CompareOrdinal(dbEmailAddress,request.EmailAdress)!=0;

            mapper.Map(request,dbUser);

            var rows = await userRepository.UpdateAsync(dbUser);

            //Check if email changed

            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAdress
                };
                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName, exchangeType: SozlukConstants.DefaultExchangeType, queueName: SozlukConstants.UserEmailChangedQueueName, obj: @event);

                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}
