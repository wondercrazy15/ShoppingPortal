
using AutoMapper;
using Data;
using Domain;
using ShoppingPortal.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingPortal.App_Start
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDetail, UserModel>();
        }
    }
}

//public static class AutoMapperConfiguration
//{
//    public static void Configure()
//    {

//        Mapper.Initialize(x => GetConfiguration(Mapper.Configuration));
//    }

//    private static void GetConfiguration(IConfiguration configuration)
//    {
//        var profiles = typeof(MappingProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
//        foreach (var profile in profiles)
//        {
//            configuration.AddProfile(Activator.CreateInstance(profile) as Profile);
//        }
//    }
//}