using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResult<ApplicationRoleDto>> ApplicationRoleCreateAsync(ApplicationRoleCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationRoleEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationRoleDto>>();
        return result;
    }
    public async Task<APIResult<ApplicationRoleDto>> ApplicationRoleEditAsync(ApplicationRoleUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationRoleEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationRoleDto>>();
        return result;
    }
    public async Task<APIResult<List<ApplicationRoleDto>>> ApplicationRolesGetAsync(ApplicationPagedRequest request)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationRoleEndpoints.Get(request));
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationRoleDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResult<ApplicationRoleDto>> ApplicationRoleGetAsync(int id)
    {
        await PrepareBearerToken();
        var result = await ApplicationRolesGetAsync(new ApplicationPagedRequest() { Id = id });
        var newResult = new APIResult<ApplicationRoleDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }

    public async Task<APIResult<string>> ApplicationRoleDeleteAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.DeleteAsync(Routes.ApplicationRoleEndpoints.Delete(id));
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
    public async Task<APIResult<string>> ApplicationRoleDisableEnableAsync(int id)
    {        
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationRoleEndpoints.DisableEnable, id);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
