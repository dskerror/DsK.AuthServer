namespace DsK.AuthServer.Client.Services.Tokens;
public interface ITokenService
{
    Task<bool> RefreshToken();
}
