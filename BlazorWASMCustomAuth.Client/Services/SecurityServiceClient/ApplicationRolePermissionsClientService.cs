using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<List<ApplicationRolePermissionGridDto>>> ApplicationRolePermissionsGetAsync(ApplicationRolePermissionsGetDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationRolePermissionsEndpoints.Get(model));
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationRolePermissionGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResult<string>> ApplicationRolePermissionChangeAsync(int roleId, int permissionId, bool permissionEnabled)
    {
        await PrepareBearerToken();
        var model = new ApplicationRolePermissionChangeDto()
        {
            PermissionId = permissionId,
            RoleId = roleId,
            PermissionEnabled = permissionEnabled
        };
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationRolePermissionsEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
