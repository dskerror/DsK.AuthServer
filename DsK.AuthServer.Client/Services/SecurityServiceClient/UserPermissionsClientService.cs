using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<List<UserPermissionGridDto>>> UserPermissionsGetAsync(int UserId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.UserPermissionEndpoints.Get(UserId));
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<UserPermissionGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<APIResult<string>> UserPermissionChangeAsync(UserPermissionChangeDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.UserPermissionEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
