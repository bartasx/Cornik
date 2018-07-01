using System;
using System.Threading.Tasks;
using Curnik.Models.IdentityModels;
using Curnik.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Curnik.Controllers
{
    public class UsersAccountsController : Controller
    {
        #region Fields 
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        #endregion

        #region Constructor
        public UsersAccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        #endregion

        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username,
                    model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Niepoprawne dane!");
                    return RedirectToAction("Login", "Home");
                }
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewAccount(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.Username,
                    AccountCreateTime = DateTime.Now,
                    LastSeenOnline = DateTime.Now,
                    AccountDescription = model.AccountDescription
                };

                var result = await this._userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this._signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Register", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}