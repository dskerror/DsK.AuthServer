using DsK.AuthServer.Security.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace DsK.AuthServer.Client.Services;
public partial class SecurityServiceClient
{  
    public async Task<APIResponse<List<ApplicationUserGridDto>>> ApplicationUsersGetAsync(int applicationId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationUserEndpoints.Get(applicationId));

        if (!response.IsSuccessStatusCode)        
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<List<ApplicationUserGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<APIResponse<string>> ApplicationUserDisableEnableAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationUserEndpoints.IsEnabledToggle, id);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
}
