using System.Diagnostics;

namespace BlazorWASMCustomAuth.Server.Models
{
	public class UserCreateLocalPasswordModel
	{
		public int UserId { get; set; }
		public string? Password { get; set; }
	}
}
