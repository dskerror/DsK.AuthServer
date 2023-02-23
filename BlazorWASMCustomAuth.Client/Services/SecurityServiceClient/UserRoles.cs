using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlazorWASMCustomAuth.Client.Services.Requests;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

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

    public async Task<APIResult<string>> UserRoleChangeAsync(int userId, int roleId, bool roleEnabled)
    {
        await PrepareBearerToken();
        var model = new UserRoleChangeDto()
        {
            UserId = userId,
            RoleId = roleId,
            RoleEnabled = roleEnabled
        };
        var response = await _httpClient.PostAsJsonAsync(Routes.UserRolesEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
