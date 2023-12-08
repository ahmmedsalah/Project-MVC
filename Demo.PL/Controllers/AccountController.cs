
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree
                    
                };
			  var Result=await _userManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                  return RedirectToAction (nameof(Login));
                foreach(var error in  Result.Errors)
                  ModelState.AddModelError(string.Empty, error.Description);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    bool flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false); // false mean  is the password is notvalid in one way the Account was locked 
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Password is Not Correct ");
                }
                ModelState.AddModelError(string.Empty, "Emil is Not Exited ");
            }

            return View(model);

        }
        #endregion

        #region Sign out 
         public new async Task<IActionResult> SignOut()
        {
             await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region ForgetPassword

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            { 
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = model.Email,Token = token }, Request.Scheme);

					var email = new Email()
                    {

                        Subject = "Reset Your Password ",
                        To = model.Email,
                        Body = resetPasswordLink
					};

                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                 }
                ModelState.AddModelError(string.Empty, "Email is not Valid");
            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion
        #region Reset Password

        public IActionResult ResetPassword(string Email , string Token)
        {
            TempData["Email"] = Email;
            TempData["Token"] = Token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel VMResetPassword)
        {
            if (ModelState.IsValid)
            {
                string Email = TempData["Email"] as string;
                string Token = TempData["Token"] as string;

                var user = await _userManager.FindByEmailAsync(Email);
                var result = await _userManager.ResetPasswordAsync(user,Token,VMResetPassword.NewPassword);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
		
            }
            return View(VMResetPassword);
        }

		#endregion
	}
}
