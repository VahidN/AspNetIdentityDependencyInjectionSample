using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentityDependencyInjectionSample.DomainClasses
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        // Add other properties here

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        public int? AddressId { get; set; }
    }
}