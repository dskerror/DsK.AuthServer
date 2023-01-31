
using Blazored.LocalStorage;
using BlazorWASMCustomAuth.Client.Security;
using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Headers;
using BlazorWASMCustomAuth.Security.Infrastructure;
using AutoMapper;

namespace BlazorWASMCustomAuth.Client.Services
{
    public class SecurityServiceClient
    {
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        public SecurityServiceClient(ILocalStorageService localStorageService,
            AuthenticationStateProvider customAuthenticationProvider,
            HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _customAuthenticationProvider = customAuthenticationProvider;
            _httpClient = httpClient;
        }
        public async Task<bool> LoginAsync(UserLoginDto model)
        {
            var response = await _httpClient.PostAsJsonAsync<UserLoginDto>("/api/security/userlogin", model);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            TokenModel tokenModel = await response.Content.ReadFromJsonAsync<TokenModel>() ?? new TokenModel("", "");
            if (tokenModel == null)
            {
                return false;
            }
            await _localStorageService.SetItemAsync("token", tokenModel.Token);
            await _localStorageService.SetItemAsync("refreshToken", tokenModel.RefreshToken);
            ((CustomAuthenticationProvider)_customAuthenticationProvider).Notify();
            return true;
        }
        public async Task<APIResultNew<UserDto>> UserCreate(UserCreateDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/security/users", model);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<APIResultNew<UserDto>>();

            return result;
        }
        public async Task<APIResultNew<UserDto>> UserEdit(UserDto model)
        {   
            var response = await _httpClient.PutAsJsonAsync("/api/security/users", model);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadFromJsonAsync<APIResultNew<UserDto>>();

            return result;
        }

        public async Task<bool> UserVerifyExistsByUsername(string username)
        {


            var response = await _httpClient.GetAsync($"/api/Security/UserVerifyExistsByUsername?Username={username}");
            if (!response.IsSuccessStatusCode)
            {
                return true;
            }

            var responseAsString = await response.Content.ReadAsStringAsync();

            try
            {
                var responseObject = JsonSerializer.Deserialize<bool>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    IncludeFields = true
                });
                return responseObject;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        public async Task<bool> UserVerifyExistsByEmail(string email)
        {


            var response = await _httpClient.GetAsync($"/api/Security/UserVerifyExistsByEmail?Email={email}");
            if (!response.IsSuccessStatusCode)
            {
                return true;
            }

            var responseAsString = await response.Content.ReadAsStringAsync();

            try
            {
                var responseObject = JsonSerializer.Deserialize<bool>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    IncludeFields = true
                });
                return responseObject;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        public async Task<APIResultNew<List<UserDto>>> UsersGet(int id = 0)
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);            

            var response = await _httpClient.GetAsync($"/api/Security/users/?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseAsString = await response.Content.ReadAsStringAsync();

            try
            {
                var responseObject = JsonSerializer.Deserialize<APIResultNew<List<UserDto>>>(responseAsString, new JsonSerializerOptions
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
        public async Task<APIResultNew<UserDto>> UserGet(int id)
        {
            var result = await UsersGet(id);
            var newResult = new APIResultNew<UserDto>
            {
                Exception = result.Exception,
                HasError = result.HasError,
                Message = result.Message,
                Result = result.Result.FirstOrDefault()
            };

            return newResult;
        }
        public async Task<bool> LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("token");
            ((CustomAuthenticationProvider)_customAuthenticationProvider).Notify();
            return true;
        }
    }
}
