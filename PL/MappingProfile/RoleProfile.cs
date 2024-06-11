using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PL.ViewModels;

namespace PL.MappingProfile
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole,RoleViewModel>().ForMember(d=>d.RoleName,o=>o.MapFrom(s=>s.Name)).ReverseMap();  
        }

    }
}
