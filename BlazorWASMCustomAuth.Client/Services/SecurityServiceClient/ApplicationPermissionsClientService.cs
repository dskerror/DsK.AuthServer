using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<ApplicationPermissionDto>> ApplicationPermissionCreateAsync(ApplicationPermissionCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationPermissionEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationPermissionDto>>();
        return result;
    }

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
