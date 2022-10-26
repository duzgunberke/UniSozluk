using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Domain.Models;
using UniSozluk.Common.Models.Queries;
using UniSozluk.Common.Models.RequestModels;

namespace UniSozluk.Api.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,LoginUserViewModel>().ReverseMap();

            CreateMap<CreateUserCommand, User>();

            CreateMap<UpdateUserCommand, User>();
        }
    }
}
