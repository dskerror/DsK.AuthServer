using System;

namespace BlazorWASMCustomAuth.Security.Shared
{
    public class UserModel
    {
        public UserModel()
        {
            Permissions = new List<string>();
        }
        public int Id { get; set; } = 0;
        public string? Username { get; set; }
		public string? Password { get; set; }
		public string? Name { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public List<string> Permissions { get; set; }

	}
}
