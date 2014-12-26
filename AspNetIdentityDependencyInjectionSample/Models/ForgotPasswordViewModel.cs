using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityDependencyInjectionSample.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}