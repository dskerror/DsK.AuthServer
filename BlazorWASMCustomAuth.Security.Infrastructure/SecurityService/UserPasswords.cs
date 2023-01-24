using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Validations;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult UserCreateLocalPassword(UserCreateLocalPasswordDto model)
        {
            //TODO : Implement Password Complexity Rules
            //TODO : Implement Previously Used Password Constraint

            APIResult result = new APIResult(model);
            int recordsCreated = 0;

            var ramdomSalt = SecurityHelpers.RandomizeSalt;

            var userPassword = new UserPassword()
            {
                Id = model.UserId,
                HashedPassword = SecurityHelpers.HashPasword(model.Password, ramdomSalt),
                Salt = Convert.ToHexString(ramdomSalt)
            };

            db.UserPasswords.Add(userPassword);

            try
            {
                recordsCreated = db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.InnerException.Message;
            }

            result.Result = userPassword;
            if (recordsCreated == 1)
            {
                result.Message = "Record Created";
            }

            return result;
        }
    }
}
