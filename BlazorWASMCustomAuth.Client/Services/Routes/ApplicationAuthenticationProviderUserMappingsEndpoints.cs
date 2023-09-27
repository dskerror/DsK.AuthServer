namespace BlazorWASMCustomAuth.Client.Services.Routes;
public static class ApplicationAuthenticationProviderUserMappingsEndpoints
{
    public static string Get(int userId)
    {
        var url = "";//$"api/ApplicationAuthenticationProviderUserMappings?ApplicationId={request.ApplicationId}?Id={request.Id}&pageNumber={request.PageNumber}&pageSize={request.PageSize}&searchString={request.SearchString}&orderBy={request.Orderby}";
        return url;
    }

    public static string Post = "api/ApplicationAuthenticationProviderUserMappings";
    public static string Put = "api/ApplicationAuthenticationProviderUserMappings";
    public static string Delete(int id)
    {
        var url = $"api/ApplicationAuthenticationProviderUserMappings?Id={id}";
        return url;
    }
}
