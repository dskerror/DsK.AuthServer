using System.Net;

namespace DsK.AuthServer.Client.Services.Tokens;

public class TokenRefreshHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public TokenRefreshHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // Check if the token is expired.
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Attempt to refresh the token.
            var tokenRefreshed = await _tokenService.RefreshToken();

            if (tokenRefreshed)
            {
                // Clone the request as HttpRequestMessage is designed to be sent only once.
                var newRequest = CloneHttpRequestMessage(request);

                // Retry the request with the new token.
                response = await base.SendAsync(newRequest, cancellationToken);
            }
        }

        return response;
    }

    private HttpRequestMessage CloneHttpRequestMessage(HttpRequestMessage oldRequest)
    {
        var newRequest = new HttpRequestMessage(oldRequest.Method, oldRequest.RequestUri)
        {
            Content = oldRequest.Content,
            Version = oldRequest.Version,
        };

        foreach (var header in oldRequest.Headers)
        {
            newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return newRequest;
    }
}