using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public static class Access
    {
        public const string Admin = "Admin";

        [DisplayName("Applications")]
        [Description("Applications Permissions")]
        public static class Application
        {
            public const string View = "Applications.View";
            public const string Create = "Applications.Create";
            public const string Edit = "Applications.Edit";
            public const string Delete = "Applications.Delete";
            public const string GenerateNewAPIKey = "Applications.GenerateNewAPIKey";
        }

        [DisplayName("Authentication Providers")]
        [Description("Authentication Providers Permissions")]
        public static class ApplicationAuthenticationProvider
        {
            public const string View = "ApplicationAuthenticationProviders.View";
            public const string Create = "ApplicationAuthenticationProviders.Create";
            public const string Edit = "ApplicationAuthenticationProviders.Edit";
            public const string Delete = "ApplicationAuthenticationProviders.Delete";
        }

        [DisplayName("Application Permissions")]
        [Description("Application Permissions Permissions")]
        public static class ApplicationPermissions
        {
            public const string View = "ApplicationPermissions.View";
            public const string Create = "ApplicationPermissions.Create";
            public const string Edit = "ApplicationPermissions.Edit";
            public const string Delete = "ApplicationPermissions.Delete";
        }
        [DisplayName("Application Role Permissions")]
        [Description("Application Role Permissions Permissions")]
        public static class ApplicationRolesPermissions
        {
            public const string View = "ApplicationRolePermissions.View";
            public const string Edit = "ApplicationRolePermissions.Edit";
        }

        [DisplayName("Application Roles")]
        [Description("Application Roles Permissions")]
        public static class ApplicationRoles
        {
            public const string View = "ApplicationRoles.View";
            public const string Create = "ApplicationRoles.Create";
            public const string Edit = "ApplicationRoles.Edit";
            public const string Delete = "ApplicationRoles.Delete";
        }

        [DisplayName("User Authentication Providers")]
        [Description("AppliUsercation Authentication Providers Permissions")]
        public static class UserAuthenticationProviders
        {
            public const string View = "UserAuthenticationProviders.View";
            public const string Create = "UserAuthenticationProviders.Create";
            public const string Edit = "UserAuthenticationProviders.Edit";
            public const string Delete = "UserAuthenticationProviders.Delete";
        }

        [DisplayName("User Passwords")]
        [Description("User Passwords Permissions")]
        public static class UserPasswords
        {            
            public const string Create = "UserPasswords.Create";           
        }


        [DisplayName("Users Permissions")]
        [Description("Users Permissions Permissions")]
        public static class UserPermissions
        {
            public const string View = "UserPermissions.View";            
            public const string Edit = "UserPermissions.Edit";            
        }


        [DisplayName("Users Roles")]
        [Description("Users Roles Permissions")]
        public static class UserRoles
        {
            public const string View = "UserRoles.View";            
            public const string Edit = "UserRoles.Edit";            
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

        [DisplayName("Application Users")]
        [Description("Application Users Permissions")]
        public static class ApplicationUsers
        {
            public const string View = "ApplicationUsers.View";
            public const string Create = "ApplicationUsers.Create";
            public const string Edit = "ApplicationUsers.Edit";
            public const string Delete = "ApplicationUsers.Delete";
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
