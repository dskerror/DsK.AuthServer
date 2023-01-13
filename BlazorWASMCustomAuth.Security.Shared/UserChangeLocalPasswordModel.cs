

namespace BlazorWASMCustomAuth.Security.Shared
{
	public class UserChangeLocalPasswordModel
    {
		public int UserId { get; set; }
		public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
