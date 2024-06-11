using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,IMapper mapper)
        {
           _userManager = userManager;
            _mapper = mapper;
        }
        

        public async Task<IActionResult> Index(string SearchValue)//master Page
        {
            if (string.IsNullOrEmpty(SearchValue)) 
            {   //Manual Mapping
                var users = await _userManager.Users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FName = u.FName,
                    LName = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).ToListAsync(); 
                return View(users);
            }

            else
            {
                var user= await _userManager.FindByEmailAsync(SearchValue);
                if (user==null)
                    return View(Enumerable.Empty<UserViewModel>());
               
                    var mappedUser = new UserViewModel()
                    {
                        Id = user.Id,
                        FName = user.FName,
                        LName = user.LName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(user).Result

                    };
                return View(new List<UserViewModel> { mappedUser });



            }


        }




        public async Task<IActionResult> Details(string id,string ViewName) 
        {
            if (id is null)
                return BadRequest();
            var user= await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();
            var mappedUser = _mapper.Map<ApplicationUser,UserViewModel>(user);
            return View(ViewName,mappedUser);

        }



        public async Task<IActionResult> Edit(string id)
        {

          
            return  await Details(id,"Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model,[FromRoute]string id)
        {
          if(model.Id != id)
                return BadRequest();

          if (ModelState.IsValid)
            {

                try
                {
                    var user =await _userManager.FindByIdAsync(id);
                    user.FName=model.FName;
                    user.LName=model.LName;
                    user.PhoneNumber=model.PhoneNumber;
                    
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty,ex.Message);
                }


            }
          return View(model);

        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task< IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var user= await _userManager.FindByIdAsync(id);
              await  _userManager.DeleteAsync(user);
                return RedirectToAction("Index");

            }
            catch (System.Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error","Home");
            }
        }




      
    }
}
