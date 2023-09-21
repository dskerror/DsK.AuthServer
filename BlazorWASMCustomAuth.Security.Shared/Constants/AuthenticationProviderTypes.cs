using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public static class AuthenticationProviderTypes
    {
        public static readonly string ActiveDirectory = "Active Directory";

        public static List<string> GetAuthenticationProviderTypes()
        {
            var list = new List<string>();
            foreach (var prop in typeof(AuthenticationProviderTypes).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    list.Add(propertyValue.ToString());
            }
            return list;
        }
    }
}
