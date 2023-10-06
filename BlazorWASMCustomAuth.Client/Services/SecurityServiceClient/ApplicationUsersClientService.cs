using BlazorWASMCustomAuth.Security.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BlazorWASMCustomAuth.Client.Services;
public partial class SecurityServiceClient
{  
    public async Task<APIResult<List<ApplicationUserGridDto>>> ApplicationUsersGetAsync(int applicationId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationUserEndpoints.Get(applicationId));

        if (!response.IsSuccessStatusCode)        
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationUserGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }
}
