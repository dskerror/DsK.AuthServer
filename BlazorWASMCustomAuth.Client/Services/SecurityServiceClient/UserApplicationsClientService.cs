using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using Azure.Core;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<List<ApplicationUserDto>>> UserApplicationsGetAsync(PagedRequest request)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.UserApplicationsEndpoints.Get(request.Id, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
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

    //public async Task<APIResult<string>> UserApplicationChangeAsync(int userId, int roleId, bool roleEnabled)
    //{
    //    await PrepareBearerToken();
    //    var model = new UserRoleChangeDto()
    //    {
    //        UserId = userId,
    //        RoleId = roleId,
    //        RoleEnabled = roleEnabled
    //    };
    //    var response = await _httpClient.PostAsJsonAsync(Routes.UserRolesEndpoints.Post, model);
    //    if (!response.IsSuccessStatusCode)
    //        return null;

    //    var result = await response.Content.ReadFromJsonAsync<APIResult<string>>();
    //    return result;
    //}
}
