﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AspNetIdentityDependencyInjectionSample.DataLayer.Context;
using AspNetIdentityDependencyInjectionSample.DomainClasses;
using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity;

namespace AspNetIdentityDependencyInjectionSample.ServiceLayer
{
    public class ApplicationRoleManager : RoleManager<CustomRole, int>, IApplicationRoleManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleStore<CustomRole, int> _roleStore;
        private readonly IDbSet<ApplicationUser> _users;
        public ApplicationRoleManager(IUnitOfWork uow, IRoleStore<CustomRole, int> roleStore)
            : base(roleStore)
        {
            _uow = uow;
            _roleStore = roleStore;
            _users = _uow.Set<ApplicationUser>();
        }

        public CustomRole FindRoleByName(string roleName)
        {
            return this.FindByName(roleName); // RoleManagerExtensions
        }

        public IdentityResult CreateRole(CustomRole role)
        {
            return this.Create(role); // RoleManagerExtensions
        }

        public IList<CustomUserRole> GetCustomUsersInRole(string roleName)
        {
            return this.Roles.Where(role => role.Name == roleName)
                             .SelectMany(role => role.Users)
                             .ToList();
            // = this.FindByName(roleName).Users
        }

        public IList<ApplicationUser> GetApplicationUsersInRole(string roleName)
        {
            var selectedUserIds = from role in this.Roles
                                  where role.Name == roleName
                                  from user in role.Users
                                  select user.UserId;
            return _users.Where(applicationUser => selectedUserIds.Contains(applicationUser.Id)).ToList();
        }

        public IList<CustomRole> FindUserRoles(int userId)
        {
            var query = from role in this.Roles
                        from user in role.Users
                        where user.UserId == userId
                        select role;

            return query.OrderBy(x => x.Name).ToList();
        }

        public string[] GetRolesForUser(int userId)
        {
            var roles = FindUserRoles(userId);
            if (roles == null || !roles.Any())
            {
                return new string[] { };
            }

            return roles.Select(x => x.Name).ToArray();
        }

        public bool IsUserInRole(int userId, string roleName)
        {
            var query = from role in this.Roles
                        where role.Name == roleName
                        from user in role.Users
                        where user.UserId == userId
                        select role;
            var userRole = query.FirstOrDefault();
            return userRole != null;
        }
    }
}