using DsK.AuthServer.Security.Shared;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace DsK.AuthServer.Client.Services;
public partial class SecurityServiceClient
{  
    public async Task<APIResponse<UserDto>> UserCreateAsync(UserCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResponse<UserDto>>();
        return result;
    }
    public async Task<APIResponse<UserDto>> UserEditAsync(UserDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResponse<UserDto>>();
        return result;
    }
    public async Task<APIResponse<List<UserDto>>> UsersGetAsync(PagedRequest request)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.UserEndpoints.Get(request.Id, request.PageNumber, request.PageSize, request.SearchString, request.OrderBy));

        if (!response.IsSuccessStatusCode)        
            return null;

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResponse<List<UserDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResponse<UserDto>> UserGetAsync(int id)
    {
        await PrepareBearerToken();
        var result = await UsersGetAsync(new PagedRequest() { Id = id});
        var newResult = new APIResponse<UserDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }
}
