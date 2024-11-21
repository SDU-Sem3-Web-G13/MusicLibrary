using System.ComponentModel.DataAnnotations;
using Models.DataAccess;
using Microsoft.Extensions.DependencyInjection;

public class User
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
        var userRepository = validationContext.GetService(typeof(UserRepository)) as UserRepository;
        var email = value as string ?? "";
        var user = validationContext.ObjectInstance as User;

        if (user != null && userRepository != null) 
        {
            if(user.SourcePage == "RegistrationPage" && userRepository.EmailExists(email))
            {
                return new ValidationResult("User with that Email already exists.");
            }
            else if(user.SourcePage == "LoginPage" && !userRepository.EmailExists(email))
            {
                return new ValidationResult("User with that Email does not exist.");
            }
        } 

        return ValidationResult.Success ?? throw new NotImplementedException();
    }
}