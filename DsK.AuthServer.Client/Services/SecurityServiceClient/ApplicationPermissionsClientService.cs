using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResponse<ApplicationPermissionDto>> ApplicationPermissionCreateAsync(ApplicationPermissionCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationPermissionEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<ApplicationPermissionDto>>();
        return result;
    }
    public async Task<APIResponse<List<ApplicationPermissionDto>>> ApplicationPermissionsGetAsync(ApplicationPagedRequest request)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationPermissionEndpoints.Get(request));
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<List<ApplicationPermissionDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResponse<ApplicationPermissionDto>> ApplicationPermissionGetAsync(int id)
    {
        await PrepareBearerToken();
        var result = await ApplicationPermissionsGetAsync(new ApplicationPagedRequest() { Id = id });
        var newResult = new APIResponse<ApplicationPermissionDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }
    public async Task<APIResponse<ApplicationPermissionDto>> ApplicationPermissionEditAsync(ApplicationPermissionUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationPermissionEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<ApplicationPermissionDto>>();
        return result;
    }

    public async Task<APIResponse<string>> ApplicationPermissionDeleteAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.DeleteAsync(Routes.ApplicationPermissionEndpoints.Delete(id));
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
    public async Task<APIResponse<string>> ApplicationPermissionDisableEnableAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationPermissionEndpoints.IsEnabledToggle, id);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
}
