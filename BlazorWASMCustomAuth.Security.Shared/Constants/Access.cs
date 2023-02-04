using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Shared.Constants
{
    public static class Access
    {
        [DisplayName("Authentication Providers")]
        [Description("Authentication Providers Permissions")]
        public static class AuthenticationProvider
        {
            public const string View = "AuthenticationProviders.View";
            public const string Create = "AuthenticationProviders.Create";
            public const string Edit = "AuthenticationProviders.Edit";
            public const string Delete = "AuthenticationProviders.Delete";
        }

        [DisplayName("Permissions")]
        [Description("Permissions Permissions")]
        public static class Permissions
        {
            public const string View = "Permissions.View";
            public const string Create = "Permissions.Create";
            public const string Edit = "Permissions.Edit";
            public const string Delete = "Permissions.Delete";
        }
        [DisplayName("Role Permissions")]
        [Description("Role Permissions Permissions")]
        public static class RolesPermissions
        {
            public const string View = "RolePermissions.View";
            public const string Create = "RolePermissions.Create";
            public const string Edit = "RolePermissions.Edit";
            public const string Delete = "RolePermissions.Delete";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Roles.View";
            public const string Create = "Roles.Create";
            public const string Edit = "Roles.Edit";
            public const string Delete = "Roles.Delete";
        }

        [DisplayName("User Passwords")]
        [Description("User Passwords Permissions")]
        public static class UserPasswords
        {
            public const string View = "UserPasswords.View";
            public const string Create = "UserPasswords.Create";
            public const string Edit = "UserPasswords.Edit";
            public const string Delete = "UserPasswords.Delete";
        }


        [DisplayName("Users Permissions")]
        [Description("Users Permissions Permissions")]
        public static class UserPermissions
        {
            public const string View = "UserPermissions.View";
            public const string Create = "UserPermissions.Create";
            public const string Edit = "UserPermissions.Edit";
            public const string Delete = "UserPermissions.Delete";
        }


        [DisplayName("Users Roles")]
        [Description("Users Roles Permissions")]
        public static class UserRoles
        {
            public const string View = "UserRoles.View";
            public const string Create = "UserRoles.Create";
            public const string Edit = "UserRoles.Edit";
            public const string Delete = "UserRoles.Delete";
        }

        [DisplayName("Users")]
        [Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
        }


        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permissions = new List<string>();
            foreach (var prop in typeof(Access).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }
    }
}
