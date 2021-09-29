using AutoMapper;
using FlashcardCreator.Domain.Entities;
using FlashcardCreator.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // user
            CreateMap<User, UserModel>();
            //.ForMember(dst => dst.Avatar, opt => opt.MapFrom(src => src.Avatar.Path));
        }
    }
}
