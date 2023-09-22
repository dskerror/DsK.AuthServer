using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResult<ApplicationAuthenticationProviderDto>> ApplicationAuthenticationProviderCreateAsync(ApplicationAuthenticationProviderCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationAuthenticationProvidersEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationAuthenticationProviderDto>>();
        return result;
    }
    public async Task<APIResult<ApplicationAuthenticationProviderDto>> ApplicationAuthenticationProviderEditAsync(ApplicationAuthenticationProviderDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationAuthenticationProvidersEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationAuthenticationProviderDto>>();
        return result;
    }
    public async Task<APIResult<List<ApplicationAuthenticationProviderDto>>> ApplicationAuthenticationProvidersGetAsync(ApplicationPagedRequest request)
    {
        await PrepareBearerToken();
        var url = Routes.ApplicationAuthenticationProvidersEndpoints.Get(request);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationAuthenticationProviderDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResult<ApplicationAuthenticationProviderDto>> ApplicationAuthenticationProviderGetAsync(int id)
    {
        var result = await ApplicationAuthenticationProvidersGetAsync(new ApplicationPagedRequest() { Id = id });
        var newResult = new APIResult<ApplicationAuthenticationProviderDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }

    public async Task<APIResult<string>> ApplicationAuthenticationProviderDeleteAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.DeleteAsync(Routes.ApplicationAuthenticationProvidersEndpoints.Delete(id));
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
    public async Task<APIResult<string>> ApplicationAuthenticationProviderDisableEnableAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationAuthenticationProvidersEndpoints.DisableEnable, id);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }
}
