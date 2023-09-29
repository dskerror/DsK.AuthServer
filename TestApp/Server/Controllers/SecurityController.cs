using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestApp.Server.HttpClients;
using TestApp.Shared;

namespace TestApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        HttpClient _Http;
        private readonly TokenSettingsModel _tokenSettings;
        public SecurityController(AuthorizarionServerAPIHttpClient authorizarionServerAPIHttpClient, IOptions<TokenSettingsModel> tokenSettings)
        {
            _Http = authorizarionServerAPIHttpClient.Client;
            _tokenSettings = tokenSettings.Value;
        }

        [HttpPost]
        [Route("ValidateLoginToken")]
        public async Task<IActionResult> ValidateLoginToken(ValidateLoginTokenDto model)
        {
            //todo : fix this line
            model.TokenKey = _tokenSettings.Key;
            var response = await _Http.PostAsJsonAsync($"https://localhost:7045/api/authentication/ValidateLoginToken", model);

            if (!response.IsSuccessStatusCode) return NotFound();

            var result = await response.Content.ReadFromJsonAsync<TokenModel>();

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}