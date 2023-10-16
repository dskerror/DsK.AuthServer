using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<List<UserRoleGridDto>>> UserRolesGetAsync(int UserId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.UserRolesEndpoints.Get(UserId));
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<UserRoleGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<APIResult<string>> UserRoleChangeAsync(int userId, int roleId, bool isEnabled)
    {
        await PrepareBearerToken();
        var model = new UserRoleChangeDto()
        {
            UserId = userId,
            RoleId = roleId,
            IsEnabled = isEnabled
        };
        var response = await _httpClient.PostAsJsonAsync(Routes.UserRolesEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
