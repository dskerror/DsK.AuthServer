using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public async Task<APIResult<string>> UserCreateLocalPassword(UserCreateLocalPasswordDto model)
        {
            //TODO : Implement Password Complexity Rules
            //TODO : Implement Previously Used Password Constraint

            APIResult<string> result = new APIResult<string>();
            int recordsCreated = 0;

            var ramdomSalt = SecurityHelpers.RandomizeSalt;

            var userPassword = new UserPassword()
            {
                UserId = model.UserId,
                HashedPassword = SecurityHelpers.HashPasword(model.Password, ramdomSalt),
                Salt = Convert.ToHexString(ramdomSalt),
                DateCreated = DateTime.Now
            };

            db.UserPasswords.Add(userPassword);

            try
            {
                recordsCreated = await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }

            if (recordsCreated == 1)
            {
                result.Result = recordsCreated.ToString();
                result.Message = "Record Created";
            }

            return result;
        }
    }
}
