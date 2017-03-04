using System.Security.Claims;
using System.Threading.Tasks;
using AspNetIdentityDependencyInjectionSample.DomainClasses;
using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace AspNetIdentityDependencyInjectionSample.ServiceLayer
{
    public class ApplicationSignInManager :
        SignInManager<ApplicationUser, int>,
        IApplicationSignInManager
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationSignInManager(
            IApplicationUserManager userManager,
            IAuthenticationManager authenticationManager) :
            base((ApplicationUserManager)userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        //{
        //    return _userManager.GenerateUserIdentityAsync(user);
        //}
    }
}