using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Application.Interfaces.Repositories;
using UniSozluk.Api.Domain.Models;
using UniSozluk.Common;
using UniSozluk.Common.Events.User;
using UniSozluk.Common.Infrastructure;
using UniSozluk.Common.Infrastructure.Exceptions;
using UniSozluk.Common.Models.RequestModels;

namespace UniSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(i => i.EmailAdress == request.EmailAdress);

            if (existUser is not null)
                throw new DatabaseValidationException("User already exists!");

            var dbUser = mapper.Map<Domain.Models.User>(request);
            
            var rows=await userRepository.AddAsync(dbUser);

            //  Email Changed/Created

            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAdress
                };
                QueueFactory.SendMessageToExchange(exchangeName:SozlukConstants.UserExchangeName,exchangeType:SozlukConstants.DefaultExchangeType,queueName:SozlukConstants.UserEmailChangedQueueName,obj:@event);
            }

            return dbUser.Id;
        }
    }
}
