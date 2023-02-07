﻿using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlazorWASMCustomAuth.Client.Services.Requests;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{

    public async Task<APIResult<List<RolePermissionGridDto>>> RolePermissionsGetAsync(int RoleId)
    {
        await PrepareBearerToken();
        var response = await _httpClient.GetAsync(Routes.RolePermissionsEndpoints.Get(RoleId));
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonSerializer.Deserialize<APIResult<List<RolePermissionGridDto>>>(responseAsString, new JsonSerializerOptions
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
}