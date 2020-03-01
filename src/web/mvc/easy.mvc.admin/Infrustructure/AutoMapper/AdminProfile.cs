//using AutoMapper;
//using Easy.Mvc.Admin.Models;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Easy.Mvc.Admin.Infrustructure.AutoMapper
//{
//    public class AdminProfile : Profile
//    {
//        public AdminProfile()
//        {
//            CreateMap<IdentityUser, UserViewModel>()
//                .ForMember(source=>source.Name,des=>des.MapFrom(_=>_.UserName))
//                .ForMember(source => source.Id, des => des.MapFrom(_ => _.Id))
//                .ForMember(source => source.Mobile, des => des.MapFrom(_ => _.))
//                .ForMember(source => source.Name, des => des.MapFrom(_ => _.UserName))
//                ;
//            CreateMap<UserViewModel, IdentityUser>();
//        }
//    }
//}
