namespace BlazorWASMCustomAuth.Security.Shared
{
	public class TokenModel
	{
        public string CallbackURL { get; set; }
        public string Token { get; set; }
		public string RefreshToken { get; set; }

		public TokenModel(string token, string refreshToken)
		{
			CallbackURL = "";
			Token = token;
			RefreshToken = refreshToken;
		}
	}
}
