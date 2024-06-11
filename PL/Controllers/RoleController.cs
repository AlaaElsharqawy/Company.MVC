using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PL.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region Index

      
        public async Task<IActionResult> Index(string SearchValue)//master Page
        {
            if (string.IsNullOrEmpty(SearchValue))
            {   //Manual Mapping
                var Roles = await _roleManager.Roles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    RoleName = r.Name,
                }).ToListAsync();
                return View(Roles);
            }
            //.FindByNameAsync(SearchValue);
            else
            {
                var Role = await _roleManager.Roles.Where(R => R.Name.ToLower().Contains(SearchValue.ToLower())).Select(r =>new  RoleViewModel{Id=r.Id,RoleName=r.Name}).ToListAsync(); 
                if (Role == null)
                    return View(Enumerable.Empty<RoleViewModel>());

              
                return View(Role);



            }


        }
        #endregion


        #region Create
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {

                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction("Index");

            }
            return View(model);
        }

        #endregion




        #region Details





        public async Task<IActionResult> Details(string id, string ViewName)
        {
            if (id is null)
                return BadRequest();
            var Role = await _roleManager.FindByIdAsync(id);

            if (Role == null)
                return NotFound();
            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(ViewName, mappedRole);

        }

        #endregion




        #region Edit

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model, [FromRoute] string id)
        {
            if (model.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);
                    Role.Name = model.RoleName;

                    await _roleManager.UpdateAsync(Role);
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }


            }
            return View(model);

        }

        #endregion


        #region Delete
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var user = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(user);
                return RedirectToAction("Index");

            }
            catch (System.Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role =await  _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound();
            ViewBag.RoleId = roleId;

            var UserInRole = new List<UserInRoleViewModel>();
            var users= await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                //manual mapping
                var MappedUser = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                //check in user in role or not

                if (await _userManager.IsInRoleAsync(user,role.Name))
                
                   MappedUser.IsSelected= true;

                else
                   MappedUser.IsSelected= false;

                UserInRole.Add(MappedUser);



            } 
            return View(UserInRole);
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId,List<UserInRoleViewModel> users)
        {
            var role =await _roleManager.FindByIdAsync(roleId);

            if(role == null)
                return NotFound();

            if (ModelState.IsValid)
            {

                foreach (var user in users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.UserId);
                    if(AppUser != null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(AppUser, role.Name))
                        
                            await _userManager.AddToRoleAsync(AppUser, role.Name);
                        
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))
                        
                            await _userManager.RemoveFromRoleAsync(AppUser, role.Name);
                        
                    }
                   
                }



                return RedirectToAction("Edit", new { id = role.Id });
            }
            return View(users);
          


        }

        #endregion










    }
}



