using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EarnIt.Models;
using EarnIt.Models.AccountViewModels;
using EarnIt.Services;

namespace EarnIt.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return BadRequest(new {error = $"You must be a logged in user to perform this action on {returnUrl}" } );
        }

      // Allows for a json post registration
        [HttpPost]
        [AllowAnonymous]
        public  async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            if(model.ConfirmPassword == model.Password)
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");

                    UserViewModel userView = new UserViewModel();
                    userView.Email = user.Email;
                    userView.Id = user.Id;

                    return Json(new { Success = "Registered successfully!", User = userView });
                }
                AddErrors(result);

                return BadRequest(new { result.Errors });
            }
            else
            {
                return BadRequest(new {error = "Passwords do not match"});
            }
        }

        // allows for a json post login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    var loggedinUser = GetCurrentUserAsync();
                    return Json( new {success = "Logged in successfully!"});
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return BadRequest( new {locked="User is locked out."} );
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest( new {invalid="Invalid login attempt."} );
                }
            }

            return BadRequest( new { failure="Unable to login user." } );
        }

        // allows API logoff and returns json response
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            try 
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation(4, "User logged out.");
                return Json(new {LoggedOut="Current user successfully logged out."});
            }
            catch
            {
                return Json(new {Failure="Something went wrong.  Not able to log out the current user."});
            }
        }

        // allows for json get of current logged in user
        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            try
            {
                var user = GetCurrentUserAsync();
                return Json(new{ user = new { id = user.Result.Id, name = user.Result.Email } } );
            }
            catch
            {
                return Json(new { failure="Unable to find a logged in user."});
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
