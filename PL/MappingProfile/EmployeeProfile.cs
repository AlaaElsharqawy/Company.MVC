using AutoMapper;
using DAL.Models;
using PL.ViewModels;

namespace PL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {

        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
        }
    }
}
