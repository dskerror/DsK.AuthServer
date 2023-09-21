using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResult<UserAuthenticationProviderMappingDto>> UserAuthenticationProviderCreateAsync(UserAuthenticationProviderCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.UserAuthenticationProvidersEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<UserAuthenticationProviderMappingDto>>();
        return result;
    }
    public async Task<APIResult<string>> UserAuthenticationProviderEditAsync(UserAuthenticationProviderUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.UserAuthenticationProvidersEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }

    public async Task<APIResult<List<ApplicationAuthenticationProviderDto>>> UserAuthenticationProvidersGetAsync(int userId)
    {
        await PrepareBearerToken();
        var url = Routes.UserAuthenticationProvidersEndpoints.Get(userId);
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
    //public async Task<APIResult<List<ApplicationUserAuthenticationProviderMappingsGridDto>>> ApplicationAuthenticationProvidersGetAsync(int UserId)
    //{
    //    await PrepareBearerToken();
    //    var response = await _httpClient.GetAsync(Routes.ApplicationAuthenticationProvidersEndpoints.Get(UserId));
    //    if (!response.IsSuccessStatusCode)
    //        return null;        

    //    var responseAsString = await response.Content.ReadAsStringAsync();

    //    try
    //    {
    //        var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationUserAuthenticationProviderMappingsGridDto>>>(responseAsString);
    //        return responseObject;
    //    }
    //    catch (Exception ex)
    //    {

    //        Console.Write(ex.Message);
    //        return null;
    //    }
    //}

    public async Task<APIResult<string>> UserAuthenticationProviderDeleteAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.DeleteAsync(Routes.UserAuthenticationProvidersEndpoints.Delete(id));
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
        return result;
    }

}
