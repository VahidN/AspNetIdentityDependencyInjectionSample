using AspNetIdentityDependencyInjectionSample.DataLayer.Context;
using AspNetIdentityDependencyInjectionSample.DomainClasses;
using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentityDependencyInjectionSample.ServiceLayer
{
    public class CustomUserStore :
        UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        ICustomUserStore
    {
        private readonly IUnitOfWork _context;

        public CustomUserStore(IUnitOfWork context)
            : base((ApplicationDbContext)context)
        {
            _context = context;
        }

    }
}