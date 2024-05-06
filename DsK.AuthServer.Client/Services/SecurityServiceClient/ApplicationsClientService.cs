using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResponse<ApplicationDto>> ApplicationCreateAsync(ApplicationCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationsEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<ApplicationDto>>();
        return result;
    }
    public async Task<APIResponse<ApplicationDto>> ApplicationEditAsync(ApplicationUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationsEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResponse<ApplicationDto>>();
        return result;
    }
    public async Task<APIResponse<string>> ApplicationGenerateNewAPIKeyAsync(ApplicationDto model)
    {   
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationsEndpoints.GenerateNewAPIKey, model);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
    public async Task<APIResponse<List<ApplicationDto>>> ApplicationsGetAsync(PagedRequest request)
    {
        await PrepareBearerToken();
        var url = Routes.ApplicationsEndpoints.Get(request);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<List<ApplicationDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResponse<ApplicationDto>> ApplicationGetAsync(int id)
    {
        var result = await ApplicationsGetAsync(new PagedRequest() { Id = id });
        var newResult = new APIResponse<ApplicationDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }
    public async Task<APIResponse<string>> ApplicationDeleteAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.DeleteAsync(Routes.ApplicationsEndpoints.Delete(id));
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
    public async Task<APIResponse<string>> ApplicationDisableEnableAsync(int id)
    {        
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationsEndpoints.IsEnabledToggle, id);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }
}
