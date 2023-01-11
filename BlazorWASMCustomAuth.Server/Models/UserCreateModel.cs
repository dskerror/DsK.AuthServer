using System;

namespace BlazorWASMCustomAuth.Server.Models
{
	public class UserCreateModel
	{
		public string? Username { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }		
	}
}
