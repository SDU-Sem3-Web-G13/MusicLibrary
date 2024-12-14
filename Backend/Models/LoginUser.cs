using System.ComponentModel.DataAnnotations;
using Backend.DataAccess;
using Backend.DataAccess.Interfaces;
using Backend.Services;

namespace Backend.Models;

public class LoginUser
{
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [UniqueEmail]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(72, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 8)]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
    public string SourcePage { get; set; } = null!;
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var LoginRegisterService = validationContext.GetService(typeof(ILoginRegisterService)) as ILoginRegisterService;
        var email = value as string ?? "";
        var user = validationContext.ObjectInstance as LoginUser;

        if (user != null && LoginRegisterService != null) 
        {
            if(user.SourcePage == "RegistrationPage" && LoginRegisterService.EmailExists(email))
            {
                return new ValidationResult("User with that Email already exists.");
            }
            else if(user.SourcePage == "LoginPage" && !LoginRegisterService.EmailExists(email))
            {
                return new ValidationResult("User with that Email does not exist.");
            }
        } 

        return ValidationResult.Success!;
    }
}