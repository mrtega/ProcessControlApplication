using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProcessControlApplication.Models;
using ProcessControlApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Registration(RegisterViewModel registration)
        {
            string result = ""; 
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registration.Email,
                    Email = registration.Email,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    PhoneNumber = registration.PhoneNumber,
                    Address = registration.Address

                };
                var isEmailInUse = userManager.Users.FirstOrDefault(x => x.Email == registration.Email);
                if (isEmailInUse != null)
                {
                    result = "2|Email already exist.";
                }
                else
                {
                    var res = await userManager.CreateAsync(user, registration.Password);


                    if (res.Succeeded)
                    {
                        if (registration.RolesEnum == 0)
                        {
                            await userManager.AddToRoleAsync(user, "StoreKeeper");
                            await signInManager.SignInAsync(user, isPersistent: false);
                            result = "1|Account created successfully.";
                        }
                        else if (registration.RolesEnum == 1)
                        {
                            await userManager.AddToRoleAsync(user, "Supervisor");
                            await signInManager.SignInAsync(user, isPersistent: false);
                            result = "1|Account created successfully.";
                        }
                        else if (registration.RolesEnum == 2)
                        {
                            await userManager.AddToRoleAsync(user, "ProductManager");
                            await signInManager.SignInAsync(user, isPersistent: false);
                            result = "1|Account created successfully.";
                        }
                        else if (registration.RolesEnum == 3)
                        {
                            await userManager.AddToRoleAsync(user, "Manager");
                            await signInManager.SignInAsync(user, isPersistent: false);
                            result = "1|Account created successfully.";
                        }
                        else
                        {
                            await signInManager.SignInAsync(user, isPersistent: false);
                            result = "1|Account created successfully.";
                        }
                        
                    }
                    else
                    {
                        result = "0|Something went wrong trying to create account.";
                    }
                }
            }
            else
            {
                result = "3|Something went wrong trying to validate account.";
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl)
        {
            string result;
            if (ModelState.IsValid)
            {

                var res = await signInManager.PasswordSignInAsync(login.Email, login.Password,  true, false);

                if (res.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        result = $"3|{returnUrl}";
                    }
                    else
                    {
                        result = "1|Account Login successfully.";
                    }
                    

                }
                else {
                    result = "0|Password and Login Combination incorrect.";
                }
            }
            else
            {
                result = "2|Invalid Login Attempt.";
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
