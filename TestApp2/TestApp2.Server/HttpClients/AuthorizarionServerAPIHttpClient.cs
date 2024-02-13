namespace TestApp2.Server.HttpClients;

public class AuthorizarionServerAPIHttpClient{
    public AuthorizarionServerAPIHttpClient(HttpClient client)
    {
        Client = client;
    }
    public HttpClient Client { get; }
}