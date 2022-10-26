using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Application.Interfaces.Repositories;
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

            mapper.Map(request,dbUser);

            var rows = await userRepository.UpdateAsync(dbUser);

            //Check if email changed

            return dbUser.Id;
        }
    }
}
