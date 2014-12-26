using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityDependencyInjectionSample.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}