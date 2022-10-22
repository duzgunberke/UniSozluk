using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Application.Interfaces.Repositories;
using UniSozluk.Common.Infrastructure;
using UniSozluk.Common.Infrastructure.Exceptions;
using UniSozluk.Common.Models.Queries;
using UniSozluk.Common.Models.RequestModels;

namespace UniSozluk.Api.Application.Features.Commands.User
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetSingleAsync(i => i.EmailAdress == request.EmailAdress);

            if (dbUser == null)
                throw new DatabaseValidationException("User not found");

            var pass=PasswordEncryptor.Encryt(request.Password);
            if (dbUser.Password != pass)
                throw new DatabaseValidationException("Password is wrong");

            if(!dbUser.EmailConfirmed)
                throw new DatabaseValidationException("Email adress is not confirmed yet");

            var result = mapper.Map<LoginUserViewModel>(dbUser);

            var claims = new Claim[] { 
                new Claim(ClaimTypes.NameIdentifier,dbUser.Id.ToString()),
                new Claim(ClaimTypes.Email,dbUser.EmailAdress.ToString()),
                new Claim(ClaimTypes.GivenName,dbUser.FirstName.ToString()),
                new Claim(ClaimTypes.Surname,dbUser.LastName.ToString()),
            };
            result.Token = GenerateToken(claims);
            return result;
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiry,
                signingCredentials: creds,
                notBefore: DateTime.Now);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
