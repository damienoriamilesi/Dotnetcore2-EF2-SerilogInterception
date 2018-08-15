using System.Threading.Tasks;
using BethaniePieShop.Models;
using BethaniePieShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    
    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByNameAsync(model.UserName);

        if(user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if(result.Succeeded)
                return RedirectToAction("Index", "Pie");
        }

        ModelState.AddModelError("", "User name/password not found");
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
            var user = new IdentityUser {UserName = model.UserName};
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
                return RedirectToAction("Index", "Pie");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Pie");
    }
}
