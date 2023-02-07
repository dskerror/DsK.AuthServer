using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlazorWASMCustomAuth.Client.Services.Requests;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<AuthenticationProviderDto>> AuthenticationProviderCreateAsync(AuthenticationProviderCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.AuthenticationProvidersEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<AuthenticationProviderDto>>();
        return result;
    }
    public async Task<APIResult<AuthenticationProviderDto>> AuthenticationProviderEditAsync(AuthenticationProviderDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.AuthenticationProvidersEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<AuthenticationProviderDto>>();
        return result;
    }
    public async Task<APIResult<List<AuthenticationProviderDto>>> AuthenticationProvidersGetAsync(PagedRequest request)
    {
        await PrepareBearerToken();
        var url = Routes.AuthenticationProvidersEndpoints.Get(request.Id, request.PageNumber, request.PageSize, request.SearchString, request.Orderby);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonSerializer.Deserialize<APIResult<List<AuthenticationProviderDto>>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                IncludeFields = true
            });

            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResult<AuthenticationProviderDto>> AuthenticationProviderGetAsync(int id)
    {
        var result = await AuthenticationProvidersGetAsync(new PagedRequest() { Id = id });
        var newResult = new APIResult<AuthenticationProviderDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }
}
