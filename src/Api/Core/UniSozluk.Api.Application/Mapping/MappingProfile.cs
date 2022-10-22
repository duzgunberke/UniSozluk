using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Domain.Models;
using UniSozluk.Common.Models.Queries;

namespace UniSozluk.Api.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,LoginUserViewModel>().ReverseMap();
        }
    }
}
