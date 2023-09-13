using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<List<RolePermissionGridDto>>> RolePermissionsGetAsync(int RoleId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.RolePermissionsEndpoints.Get(RoleId));
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<RolePermissionGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<APIResult<string>> RolePermissionChangeAsync(int roleId, int permissionId, bool permissionEnabled)
    {
        await PrepareBearerToken();
        var model = new RolePermissionChangeDto()
        {
            PermissionId = permissionId,
            RoleId = roleId,
            PermissionEnabled = permissionEnabled
        };
        var response = await _httpClient.PostAsJsonAsync(Routes.RolePermissionsEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
