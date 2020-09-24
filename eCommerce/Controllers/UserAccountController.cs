using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.BusinessLogic;
using eCommerce.DataAccess;
using eCommerce.Models.UserAccountVM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly UserAccountService UserAccountService;
        private readonly IMapper Mapper;

        public UserAccountController(UserAccountService userAccountService, 
                                     IMapper mapper)
        { 
            UserAccountService = userAccountService;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterVM();
            return View("Register", model);
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Mapper.Map<UserT>(model);

            UserAccountService.RegisterNewUser(user);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginVM();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserAccountService.Login(model.Email, model.Password);

            if (user == null)
            {
                model.AreCredentialsInvalid = true;

                return View(model);
            }

            await LogIn(user);

            return Json(new{
                flag = true
            }) ;
        }

        private async Task LogIn(UserT user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
            };

            user.UserRole
                .ToList()
                .ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Role.RoleName)));

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                    scheme: "eCommerceCookies",
                    principal: principal);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LogOut();

            return RedirectToAction("Index", "Home");
        }

        private async Task LogOut()
        {
            await HttpContext.SignOutAsync(scheme: "eCommerceCookies");
        }
    }
}
