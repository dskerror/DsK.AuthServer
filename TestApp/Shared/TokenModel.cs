namespace TestApp.Shared;
public class TokenModel
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }

    public TokenModel(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}