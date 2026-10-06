using EmployeeManagmeent.Services;
using EmployeeManagmeent.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagmeent.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction( "Index", "Dashboard");
            }

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = await _accountService.ValidateAdminAsync(model.Email, model.Password);

            if (admin == null)
            {
                ModelState.AddModelError( string.Empty, "Invalid email or password.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, admin.Id.ToString()),

                new Claim( ClaimTypes.Name, admin.Name),

                new Claim( ClaimTypes.Email, admin.Email),

                new Claim( ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(  claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties =
                                    new AuthenticationProperties
                                    {
                                        IsPersistent = model.RememberMe
                                    };

            await HttpContext.SignInAsync( CookieAuthenticationDefaults .AuthenticationScheme, principal, authenticationProperties);

            return RedirectToAction( "Index",  "Dashboard");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
