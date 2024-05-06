using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;
public partial class SecurityServiceClient
{
    public async Task<APIResponse<string>> ApplicationAuthenticationProviderUserMappingEditAsync(ApplicationAuthenticationProviderUserMappingUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }

    public async Task<APIResponse<List<ApplicationAuthenticationProviderUserMappingsGridDto>>> ApplicationAuthenticationProviderUserMappingsGetAsync(int applicationId, int applicationUserId)
    {
        await PrepareBearerToken();
        var url = Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Get(applicationId, applicationUserId);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<List<ApplicationAuthenticationProviderUserMappingsGridDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<bool> ApplicationAuthenticationProviderUserMappingIsEnabledToggle(int applicationAuthenticationProviderId, int applicationUserId, int applicationId, bool isEnabled)
    {
        await PrepareBearerToken();
        var model = new ApplicationAuthenticationProviderUserMappingIsEnabledToggleDto()
        {
            ApplicationAuthenticationProviderId = applicationAuthenticationProviderId,
            ApplicationUserId = applicationUserId
        };
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.IsEnabledToggle, model);
        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async Task<APIResponse<string>> ApplicationAuthenticationProviderUserMappingUpdate(ApplicationAuthenticationProviderUpdateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationAuthenticationProviderUserMappingsEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResponse<string>>();
        return result;
    }

}
