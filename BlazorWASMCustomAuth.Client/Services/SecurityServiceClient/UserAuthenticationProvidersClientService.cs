using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResult<ApplicationAuthenticationProviderUserMappingDto>> ApplicationAuthenticationProviderUserMappingCreateAsync(ApplicationAuthenticationProviderUserMappingCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationAuthenticationProviderUserMappingDto>>();
        return result;
    }
    public async Task<APIResult<string>> ApplicationAuthenticationProviderUserMappingEditAsync(ApplicationAuthenticationProviderUserMappingUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }

    public async Task<APIResult<List<ApplicationAuthenticationProviderDto>>> ApplicationAuthenticationProviderUserMappingsGetAsync(int userId)
    {
        await PrepareBearerToken();
        var url = Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Get(userId);
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
    public async Task<APIResult<string>> ApplicationAuthenticationProviderUserMappingDeleteAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.DeleteAsync(Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Delete(id));
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }

}
