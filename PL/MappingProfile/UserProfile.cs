using AutoMapper;
using DAL.Models;
using PL.ViewModels;

namespace PL.MappingProfile
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser,UserViewModel>().ReverseMap();
        }
    }
}
