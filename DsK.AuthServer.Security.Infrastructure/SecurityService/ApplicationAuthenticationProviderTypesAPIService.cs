using DsK.AuthServer.Security.Shared;

namespace DsK.AuthServer.Security.Infrastructure;
public partial class SecurityService
{
    public async Task<APIResult<List<ApplicationAuthenticationProviderTypeDto>>> ApplicationAuthenticationProvidersGet()
    {
        var result = new APIResult<List<ApplicationAuthenticationProviderTypeDto>>();
        
        var list = new List<ApplicationAuthenticationProviderTypeDto>();
        list.Add(new ApplicationAuthenticationProviderTypeDto() { Name = "Local" });
        list.Add(new ApplicationAuthenticationProviderTypeDto() { Name = "Active Directory" });
        list.Add(new ApplicationAuthenticationProviderTypeDto() { Name = "Azure AD" });

        return result;
    }

    public async Task<bool> ApplicationAuthenticationProviderTypeNameIsValid(string applicationAuthenticationProviderTypeName)
    {
        if (applicationAuthenticationProviderTypeName == "Local" || 
            applicationAuthenticationProviderTypeName == "Active Directory" ||
            applicationAuthenticationProviderTypeName == "Azure AD")
            return true;
        else
            return false;
    }

}
