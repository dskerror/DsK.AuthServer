using Microsoft.AspNetCore.Mvc;
using TestApp.Server.HttpClients;
using TestApp.Shared;

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

        [HttpGet]
        [Route("ValidateLoginToken")]
        public async Task<IActionResult> ValidateLoginToken(ValidateLoginTokenDto model)
        {
            model.TokenKey = "ThisIsTheTestAppKey";
            var response = await _Http.PostAsJsonAsync($"https://localhost:7190/ValidateLoginToken", model);

            if (!response.IsSuccessStatusCode) return NotFound();

            var result = await response.Content.ReadFromJsonAsync<TokenModel>();

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}