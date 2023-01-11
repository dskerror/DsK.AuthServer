namespace BlazorWASMCustomAuth.Server.Models
{
	public class TokenSettingsModel
	{
		public string? Issuer { get; set; }
		public string? Audience { get; set; }
		public string? Key { get; set; }
	}
}
