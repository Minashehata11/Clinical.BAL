using Clinical.DAL.Context;
using Clinical.DAL.Entities;
using Clinical.DAL.Entities.Identity;
using Clinical.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Clinical.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IToastNotification _toast;
        private readonly ApplicationDbContext _contex;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, IToastNotification toast, ApplicationDbContext contex)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _toast = toast;
            _contex = contex;

        }
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View( new SignUpViewModel() );
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = data.Email.Split("@")[0],
                    Email = data.Email,
                    FirstName = data.Email.Split("@")[0]
                    

                };
                if(data.Password != data.ConfirmedPassword)
                {
                    ModelState.AddModelError("", "not matched");
                    return View(data);
                }
                var result= await _userManager.CreateAsync(user, data.Password);
                if (result.Succeeded)
                {
                    var patient = new Patient
                    {
                        PatientId=user.Id,
                        Address = data.Address,
                    };
                    
                    _contex.Add(patient);
                    
                    await _userManager.AddToRoleAsync(user, "Patient"); // Assuming adding to role after successful login

                try
                {

                     await _contex.SaveChangesAsync();
                    _toast.AddSuccessToastMessage("Register Has Confirmed");
                    return RedirectToAction("SignIn");
                }
                catch (DbUpdateException ex)
                {
                    // Handle potential database saving errors
                    ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                    // Log the exception details for further investigation
                    _logger.LogError(ex, "Error saving user and admin data");
                    return View(data);
                }

                }
                else
                {
                    // Handle user creation errors (e.g., add error messages to ModelState)
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }

                    return View(data);
                }
            }
            return View(data);
        }
		public async Task<IActionResult> SignIn()
		{
			return View(new SignInViewModel());
		}

		[HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(data.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username or Password"); // Generic error message
                    return View(data);
                }

                // Check for lockout before password check
                if (await _userManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked out. Please try again later.");
                    return View(data);
                }

                var result = await _signInManager.PasswordSignInAsync(user, data.Password, data.Remeberme, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                 
                    _logger.LogInformation("User {userEmail} successfully signed in.", user.Email); // Log successful sign-in
                                                                                                   
                     await _userManager.ResetAccessFailedCountAsync(user); // Reset failed attempts on successful login
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Increment failed attempts only if lockout is not already enabled
                    if (!await _userManager.IsLockedOutAsync(user))
                    {
                        await _userManager.AccessFailedAsync(user);
                    }

                    // Check for lockout after attempted login
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Your account is locked out for 1 minute due to multiple failed login attempts. Please try again later.");
                        return View(data);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Username or Password"); // Generic error message
                    }
                }
            }

            return View(data);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
           return  RedirectToAction(nameof(SignIn));
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        //Todo:Forget Password 
	}
}
