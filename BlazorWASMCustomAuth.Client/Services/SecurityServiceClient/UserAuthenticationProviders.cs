using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlazorWASMCustomAuth.Client.Services.Requests;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<UserAuthenticationProviderDto>> UserAuthenticationProviderCreateAsync(UserAuthenticationProviderCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.UserAuthenticationProvidersEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<UserAuthenticationProviderDto>>();
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
    public async Task<APIResult<List<UserAuthenticationProvidersGridDto>>> UserAuthenticationProvidersGetAsync(int UserId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.UserAuthenticationProvidersEndpoints.Get(UserId));
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<UserAuthenticationProvidersGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }

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
