using System.ComponentModel;
using System.Reflection;

namespace TestApp.Shared;
public static class Access
{
    public const string Admin = "Admin";

    [DisplayName("Counter")]
    [Description("Counter Permissions")]
    public static class CounterPage
    {
        public const string CounterFunction = "TestApp.Counter";        
    }

    [DisplayName("Fetch Data")]
    [Description("Fetch Data Permissions")]
    public static class FetchDataPage
    {
        public const string FetchDataFunction = "TestApp.FetchData";
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

