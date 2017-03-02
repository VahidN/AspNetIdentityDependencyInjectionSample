using System.Data.Entity;
using AspNetIdentityDependencyInjectionSample.DataLayer.Context;
using AspNetIdentityDependencyInjectionSample.DomainClasses;
using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentityDependencyInjectionSample.ServiceLayer
{
    public class CustomRoleStore :
        RoleStore<CustomRole, int, CustomUserRole>,
        ICustomRoleStore
    {
        private readonly IUnitOfWork _context;

        public CustomRoleStore(IUnitOfWork context)
            : base((DbContext)context)
        {
            _context = context;
        }
    }
}