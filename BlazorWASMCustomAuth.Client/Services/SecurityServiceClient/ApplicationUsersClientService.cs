using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using BlazorWASMCustomAuth.Security.Shared.Requests;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{  
    //public async Task<APIResult<UserDto>> UserCreateAsync(UserCreateDto model)
    //{
    //    await PrepareBearerToken();
    //    var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.Post, model);
    //    if (!response.IsSuccessStatusCode)        
    //        return null;
        
    //    var result = await response.Content.ReadFromJsonAsync<APIResult<UserDto>>();
    //    return result;
    //}
    //public async Task<APIResult<UserDto>> UserEditAsync(UserDto model)
    //{
    //    await PrepareBearerToken();
    //    var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.Put, model);
    //    if (!response.IsSuccessStatusCode)        
    //        return null;
        
    //    var result = await response.Content.ReadFromJsonAsync<APIResult<UserDto>>();
    //    return result;
    //}
    public async Task<APIResult<List<ApplicationUserDto>>> ApplicationUsersGetAsync(ApplicationPagedRequest request)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.ApplicationUserEndpoints.Get(request));

        if (!response.IsSuccessStatusCode)        
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationUserDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }
    //public async Task<APIResult<UserDto>> UserGetAsync(int id)
    //{
    //    var result = await UsersGetAsync(new PagedRequest() { Id = id});
    //    var newResult = new APIResult<UserDto>
    //    {
    //        Exception = result.Exception,
    //        HasError = result.HasError,
    //        Message = result.Message,
    //        Result = result.Result.FirstOrDefault()
    //    };

    //    return newResult;
    //}
}
