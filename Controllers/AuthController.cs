using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Models.DataAccess;

namespace RazorMusic.Controllers;
public class AuthController : Controller
{
    internal UserRepository userRepository;

    public AuthController()
    {
        userRepository = new UserRepository();
    }


    [HttpPost("login")]
    public IActionResult Login(string email, string password)
    {
        var emailHash = BCrypt.Net.BCrypt.HashPassword(email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        bool isValidUser = userRepository.VerifyUserCredentials(emailHash, passwordHash);

        if (!isValidUser)
        {
            return Unauthorized(new { success = false, message = "Invalid Username or Password" });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            
        };

        HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok(new { success = true });
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { success = true });
    }
}