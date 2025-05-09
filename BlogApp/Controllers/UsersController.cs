using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (isUser != null)
                {
                    var userClaims = new List<Claim>
                   {
                       new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()),
                       new Claim(ClaimTypes.Name, isUser.UserName ?? ""),
                       new Claim(ClaimTypes.GivenName, isUser.UserName ?? ""),
                       new Claim(ClaimTypes.UserData, isUser.Image ?? "")
                   };

                    if (isUser.Email == "okankoca@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    if(_userRepository.Users.FirstOrDefault(u=> u.Email == model.Email) == null)
                    {
                        ModelState.AddModelError("", "There is no such account with this email address");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your email address or password is wrong.");
                    }
                        
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.UserName == model.UserName);

                if(isUser == null)
                {
                    var user = new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "anon.jpg" // default anonim görseli
                    };

                    _userRepository.CreateUser(user);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "This email address or username is already in use.");
                }
            }
            return View();
        }
    }
}
