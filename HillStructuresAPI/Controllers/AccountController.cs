using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Owin;
using Microsoft.Extensions.Logging;
using HillStructuresAPI.Models;
using HillStructuresAPI.Services;

namespace HillStructuresAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<SuperUser> _signInManager;    
        private readonly UserManager<SuperUser> _userManager;
        private readonly ILogger<Register> _logger;

        public AccountController(
            UserManager<SuperUser> userManager,
            SignInManager<SuperUser> signInManager,
            ILogger<Register> logger)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _logger = logger;
            }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction("Index", "Home");
            }
            /* if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }*/
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                //fix to redirect to correct page************************
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { code = code }, protocol: Request.Scheme);

            var emailSend = new EmailSender();
                emailSend.SendEmailAsync(
                Email,
                "Reset Password",
                $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset");
            }
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string code, string email, string password, string confirmPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/AddManager
        [AllowAnonymous]
        public ActionResult AddManager()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return View();
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }            
        }

        // POST: /Account/AddManager
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManager(string Email)
        {
            if(_signInManager.IsSignedIn(User))
            {
                if (Email == null)
                {
                    //fix to redirect to correct page************************
                    return RedirectToAction("ManagerInviteSent");
                }
                else {
                    var createUser = new SuperUser { UserName = Email, Email = Email };
                    var result = await _userManager.CreateAsync(createUser, "!@#HillStructures123");
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(createUser.Email);
                        //validate that a manager user is sending invite
                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = Url.Action("SetPassword", "Account",
                            new { code = code, email = user.Email }, protocol: Request.Scheme);

                        var emailSend = new EmailSender();
                        emailSend.SendEmailAsync(
                            Email,
                            "Create an Account",
                            $"You have been invited to the Hill Structures App. Please start by <a href='{callbackUrl}'>clicking here</a>");

                        return RedirectToAction("ManagerInviteSent");
                    }
                    else {
                        foreach (var error in result.Errors)
                        {
                             ModelState.AddModelError(string.Empty, error.Description);
                        }

                        return View();
                    }
                }
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }
        }

        // GET:/Account/ManagerInviteSent
        [AllowAnonymous]
        public ActionResult ManagerInviteSent()
        {
            return View();
        }

        // GET:/Account/SetPassword
        [AllowAnonymous]
        public ActionResult SetPassword(string code, string email)
        {
            if (code == null || email == null)
            {
                return BadRequest("A code must be supplied for password reset");
            }
            return View();
        }

        // POST: /Account/SetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(string code, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

    }
}