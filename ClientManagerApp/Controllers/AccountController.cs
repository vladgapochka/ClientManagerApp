using ClientManagerApp.Models;
using ClientManagerApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagerApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        //Метод входа в систему
        public IActionResult Login()
        {
            return View();
        }

        // Авторизация пользователя в системе
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName!, model.Password!, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Clients");
                }
                
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model); 
            }
            return View(model); 
        }


        //Метод регистрации
        public IActionResult Register()
        {
            return View();
        }


        //Регистраци пользователя в системе
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
                {
                AppUser user = new()
                {
                    Name = model.Name,
                    UserName = model.Name,
                    Email = model.Email,
                    Address = model.Address
                };
                var result = await userManager.CreateAsync(user,model.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user,false);

                    return RedirectToAction("Index", "Clients");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
                return View(model);
        }

        //Методы выхода из системы
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Clients");
        }

    }
}
