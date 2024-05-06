using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;
public partial class SecurityServiceClient
{  
    public async Task<APIResponse<UserDto>> MyProfileEditAsync(UserDto model)
    {
        MyProfileUpdateDto mapping = new MyProfileUpdateDto()
        {
            Name = model.Name
        };

        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.MyProfileEndpoints.Put, mapping);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResponse<UserDto>>();
        return result;
    }
    public async Task<bool> MyProfileChangePasswordAsync(MyProfileChangePasswordDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.MyProfileEndpoints.ChangePassword, model);
        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }
    public async Task<APIResponse<UserDto>> MyProfileGetAsync()
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.MyProfileEndpoints.Get());

        if (!response.IsSuccessStatusCode)        
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<UserDto>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }    
}
