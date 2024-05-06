using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using Azure.Core;

namespace DsK.AuthServer.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResponse<List<UserApplicationGridDto>>> UserApplicationsGetAsync(int UserId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.UserApplicationsEndpoints.Get(UserId));
        
        
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<List<UserApplicationGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<APIResponse<string>> UserApplicationChangeAsync(int userId, int applicationId, bool isEnabled)
    {
        await PrepareBearerToken();
        var model = new ApplicationUserChangeDto()
        {
            ApplicationId = applicationId,
            UserId = userId,
            IsEnabled = isEnabled
        };
        var response = await _httpClient.PostAsJsonAsync(Routes.UserApplicationsEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
}
