using System.ComponentModel;
using System.Reflection;

namespace BlazorWASMCustomAuth.Security.Shared;
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
        public const string IsEnabledToggle = "Applications.IsEnabledToggle";
    }

    [DisplayName("Authentication Providers")]
    [Description("Authentication Providers Permissions")]
    public static class ApplicationAuthenticationProvider
    {
        public const string View = "ApplicationAuthenticationProviders.View";
        public const string Create = "ApplicationAuthenticationProviders.Create";
        public const string Edit = "ApplicationAuthenticationProviders.Edit";
        public const string Delete = "ApplicationAuthenticationProviders.Delete";
        public const string IsEnabledToggle = "ApplicationAuthenticationProviders.IsEnabledToggle";
    }

    [DisplayName("Application Permissions")]
    [Description("Application Permissions Permissions")]
    public static class ApplicationPermissions
    {
        public const string View = "ApplicationPermissions.View";
        public const string Create = "ApplicationPermissions.Create";
        public const string Edit = "ApplicationPermissions.Edit";
        public const string Delete = "ApplicationPermissions.Delete";
        public const string IsEnabledToggle = "ApplicationPermissions.IsEnabledToggle";            
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
        public const string IsEnabledToggle = "ApplicationRoles.IsEnabledToggle";
    }

    [DisplayName("User Authentication Providers")]
    [Description("AppliUsercation Authentication Providers Permissions")]
    public static class ApplicationAuthenticationProviderUserMappings
    {
        public const string View = "ApplicationAuthenticationProviderUserMappings.View";
        public const string Create = "ApplicationAuthenticationProviderUserMappings.Create";
        public const string Edit = "ApplicationAuthenticationProviderUserMappings.Edit";
        public const string Delete = "ApplicationAuthenticationProviderUserMappings.Delete";
    }

    [DisplayName("Users Applications")]
    [Description("Users Applications Permissions")]
    public static class UserApplications
    {
        public const string View = "UserApplications.View";
        public const string Edit = "UserApplications.Edit";
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


    [DisplayName("My Profile")]
    [Description("My Profile Permissions")]
    public static class MyProfile
    {
        public const string View = "MyProfile.View";
        public const string Edit = "MyProfile.Edit";
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