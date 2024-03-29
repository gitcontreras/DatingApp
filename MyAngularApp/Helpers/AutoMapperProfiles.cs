﻿using AutoMapper;
using MyAngularApp.DTOs;
using MyAngularApp.Entities;
using MyAngularApp.Extensions;

namespace MyAngularApp.Helpers
{
    public class AutoMapperProfiles:Profile
    {

        public AutoMapperProfiles() 
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>
                  src.Photos.FirstOrDefault(x=>x.IsMain).Url))
                .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
