﻿using BlazorWASMCustomAuth.Security.Shared;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlazorWASMCustomAuth.Client.Services.Requests;
using Newtonsoft.Json;

namespace BlazorWASMCustomAuth.Client.Services;

public partial class SecurityServiceClient
{
    public async Task<APIResult<ApplicationDto>> ApplicationCreateAsync(ApplicationCreateDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PostAsJsonAsync(Routes.ApplicationsEndpoints.Post, model);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationDto>>();
        return result;
    }
    public async Task<APIResult<ApplicationDto>> ApplicationEditAsync(ApplicationDto model)
    {
        await PrepareBearerToken();
        var response = await _httpClient.PutAsJsonAsync(Routes.ApplicationsEndpoints.Put, model);
        if (!response.IsSuccessStatusCode)        
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<APIResult<ApplicationDto>>();
        return result;
    }
    public async Task<APIResult<List<ApplicationDto>>> ApplicationsGetAsync(PagedRequest request)
    {
        await PrepareBearerToken();
        var url = Routes.ApplicationsEndpoints.Get(request.Id, request.PageNumber, request.PageSize, request.SearchString, request.Orderby);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;        

        var responseAsString = await response.Content.ReadAsStringAsync();

        try
        {
            var responseObject = JsonConvert.DeserializeObject<APIResult<List<ApplicationDto>>>(responseAsString);
            return responseObject;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
            return null;
        }
    }
    public async Task<APIResult<ApplicationDto>> ApplicationGetAsync(int id)
    {
        var result = await ApplicationsGetAsync(new PagedRequest() { Id = id });
        var newResult = new APIResult<ApplicationDto>
        {
            Exception = result.Exception,
            HasError = result.HasError,
            Message = result.Message,
            Result = result.Result.FirstOrDefault()
        };

        return newResult;
    }
}