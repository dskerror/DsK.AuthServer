using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;
public partial class SecurityServiceClient
{  
    public async Task<APIResult<UserDto>> MyProfileEditAsync(UserDto model)
    {
        MyProfileUpdateDto mapping = new MyProfileUpdateDto()
        {
            Id = model.Id,
            Name = model.Name
        };

        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.MyProfileEndpoints.Put, mapping);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<UserDto>>();
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
    public async Task<APIResult<UserDto>> MyProfileGetAsync(int id)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.MyProfileEndpoints.Get(id));

        if (!response.IsSuccessStatusCode)        
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<UserDto>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }    
}
