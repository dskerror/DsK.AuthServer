using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<List<ApplicationPermissionGridDto>>> ApplicationPermissionsGetAsync(int applicationId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationPermissionEndpoints.Get(applicationId));
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationPermissionGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
}
