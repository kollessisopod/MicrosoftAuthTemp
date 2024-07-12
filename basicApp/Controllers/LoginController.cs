using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using basicApp;
using basicApp.Controllers;
using basicApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.EntityFrameworkCore;
public class AccountController : Controller
{
    
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    //** This is the common login method. 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        return View(model);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

	//Above logic should already be in place in Atlas project. Below is the code for Microsoft Account Linking and Login
	//****************************************************************************************************************
	//** IMPORTANT: Microsoft Account Linking
	//** VERY IMPORTANT: <MicrosoftAccountDefaults.AuthenticationScheme> or "Microsoft" is the name of the authentication scheme that is used for Microsoft Account authentication.
    //** We have configured this in Program.cs file. -> builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

	[HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LinkMicrosoftAccount()
    {
        //** This is the method that is called when the user clicks on the "Link Microsoft Account" button
        //** It will redirect the user to Microsoft Account login page
        var redirectUrl = Url.Action("LinkMicrosoftAccountCallback", "Account");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Microsoft", redirectUrl);
        return Challenge(properties, "Microsoft");
        //** When the user logs in with Microsoft Account, the user will be redirected back to the "LinkMicrosoftAccountCallback" method (below)

    }
    public async Task<IActionResult> LinkMicrosoftAccountCallback()
    {
        //**console debug
        //**Console.WriteLine("LinkMicrosoftAccountCallback SUCCESSFULY FIRED");
        //** This is the method that is called when the user is redirected back to the application after logging in with Microsoft Account

        //** Get the external login info (Microsoft login credentials)
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            // Handle error
            return RedirectToAction(nameof(Index), "Home");
        }

        //** Get the user who is currently logged in
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            // Handle error
            return RedirectToAction(nameof(Index), "Home");
        }

        //** LINKING LOGIC: Update the user with the Microsoft Account credentials.
        user.MicrosoftAccountId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        user.MicrosoftAccountEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

        //** If user saving fails, which happens when user linking fails, handle error
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            Console.WriteLine("ERROR LINKING USER");
            return RedirectToAction(nameof(Index), "Home");
        }

        return RedirectToAction(nameof(Index), "Home");
    }

    //** IMPORTANT: Microsoft Account Login
    public IActionResult ExternalLogin()
    {
        var redirectUrl = Url.Action("LoginMicrosoftAccountCallback", "Account");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Microsoft", redirectUrl);
        return Challenge(properties, MicrosoftAccountDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> LoginMicrosoftAccountCallback()
    {
        Console.WriteLine("LoginMicrosoftAccountCallback SUCCESSFULY FIRED");
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            // Handle error
            return RedirectToAction(nameof(Index), "Home");
        }

        //** LINK FINDING: Check if a user is already linked to this Microsoft account
        var microsoftAccountId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var microsoftAccountEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

        var existingUser = await _userManager.Users
            .FirstOrDefaultAsync(u => u.MicrosoftAccountId == microsoftAccountId);


        if (existingUser != null)
        {
            //** If user is already linked, sign in the user
            await _signInManager.SignInAsync(existingUser, isPersistent: false);
            return RedirectToAction(nameof(Index), "Home");
        } else
        {
            // Handle error
            Console.WriteLine("ERROR LOGGING USER");
            return RedirectToAction(nameof(Index), "Home");
        }
    }

    //** Microsoft Account Unlinking, a use case scenario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnlinkMicrosoftAccount()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            // Handle error
            return RedirectToAction(nameof(Index), "Home");
        }

        user.MicrosoftAccountId = "";
        user.MicrosoftAccountEmail = "";

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            // Handle error
            Console.WriteLine("ERROR UNLINKING USER");
            return RedirectToAction(nameof(Index), "Home");
        }

        return RedirectToAction(nameof(Index), "Home");
    }

}





