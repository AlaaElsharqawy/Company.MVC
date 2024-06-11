using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel.Resolution;
using PL.Helpers;
using PL.ViewModels;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}


      
        #region Register



        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)//server side validation
            {
                //Manual Mapping
                var User = new ApplicationUser()
                {

                    UserName = model.Email.Split("@")[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };


                var Result = await _userManager.CreateAsync(User, model.Password);

                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var Error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, Error.Description);
                    }
                }

            }

            return View(model);
        }
        #endregion



        
        #region Login

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
            {
              var user=await  _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
              var Result  = await _userManager.CheckPasswordAsync(user, model.Password);

                    if(Result)
                    {
                        //Login
                   var LoginResult=   await  _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        
                        if (LoginResult.Succeeded)
                        {
                            return RedirectToAction("Index","Home");
                        }
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Password Is Incorrect");
					}
				}

				else
				{
					ModelState.AddModelError(string.Empty, "Email Not Found");
				}
			}

			return View(model);
		}









        #endregion




        #region Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
        #endregion





        #region Forget Password

        //Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        // SendEmail

        public async Task<IActionResult>  SendEmail(ForgetPasswordViewModel model)
        {
          if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    //generate token(Invalid for only one time for this user)

                    var token= await _userManager.GeneratePasswordResetTokenAsync(user);

                    //generate ResetPasswordLink(//https://localhost:4431/Account/ResetPassword?email=alaaali6101999@gmail.com token=dfghjkldketdyghjvklgfkdjihgytfgy)

                    var ResetPasswordLink = Url.Action("ResetPassword","Account",new
                    {
                      email=user.Email,
                      Token= token
                    },Request.Scheme );



                    var email = new Email()
                    {
                        To = user.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink  //ResetPasswordLink 
                    };

                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInBox));




                }
                else
                {
                    ModelState.AddModelError(string.Empty," Email Is Not Exists");
                }


            }
            return View("ForgetPassword", model);

        }
         
        //Check Your In Box
        public IActionResult CheckYourInBox()
        {
            return View();
        }



        //Reset Password

        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();


        }



        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
          if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;

               
                var user=await _userManager.FindByEmailAsync(email);

            var Result=  await  _userManager.ResetPasswordAsync(user,token,model.NewPassword);

                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach(var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty,error.Description);
                    }
                }
            }
            return View(model);
        }






        #endregion





        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
