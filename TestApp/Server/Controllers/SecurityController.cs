using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TestApp.Server.HttpClients;
using TestApp.Shared;
using static System.Net.WebRequestMethods;

namespace TestApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        HttpClient _Http;
        public SecurityController(AuthorizarionServerAPIHttpClient authorizarionServerAPIHttpClient)
        {
            _Http = authorizarionServerAPIHttpClient.Client;
        }

        //[HttpGet]
        //[Route("ApplicationLoginRequest")]
        //public async Task<string> ApplicationLoginRequest()
        //{
        //    string result = "";

        //    ApplicationLoginRequestDto model = new ApplicationLoginRequestDto()
        //    {
        //        ApplicationAuthenticationProviderGuid = "xxx",
        //        AppApiKey = "123"
        //    };
        //    try
        //    {
        //        var response = await _Http.PostAsJsonAsync("api/Authentication/ApplicationLoginRequest", model);
                
        //        if (!response.IsSuccessStatusCode)
        //            return null;

        //        result = await response.Content.ReadAsStringAsync();                

        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex);
        //    }

        //    return result;
        //}

        [HttpGet]
        [Route("ApplicationLoginApproved")]
        public async Task<string> ApplicationLoginApproved(string applicationAuthenticationProviderGUID)
        {
            string requestologintokenurl = "https://localhost:7045/requestlogintoken";
            string? result = await _Http.GetFromJsonAsync<string>(requestologintokenurl);
            return result;
        }
    }



    public class ApplicationLoginRequestDto
    {
        public string ApplicationAuthenticationProviderGuid { get; set; }

        public string AppApiKey { get; set; }
    }
}