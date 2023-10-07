namespace BlazorWASMCustomAuth.Security.Shared;
public partial class ApplicationDto
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public Guid ApplicationGuid { get; set; }
    public string ApplicationName { get; set; } = null!;
    public string ApplicationDesc { get; set; } = null!;
    public string AppApiKey { get; set; } = null!;
    public string CallbackURL { get; set; }    
}
