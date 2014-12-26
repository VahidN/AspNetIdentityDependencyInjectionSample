using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityDependencyInjectionSample.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}