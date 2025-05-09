using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Name must be at least {2} and {0} characters long.", MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")] 
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
