using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Validations;

namespace BlazorWASMCustomAuth.SecurityEF.Infrastructure
{
    public partial class SecurityServiceEF
    {
        public APIResult UserCreate(UserCreateDto model)
        {
            APIResult result = new APIResult(model);

            var newUser = new User()
            {
                Username = model.Username,
                Email = model.Email,
                Name = model.FullName
            };

            db.Users.Add(newUser);
            db.SaveChanges();
            result.Result = newUser;

            return result;
        }

        public APIResult UsersGet()
        {
            APIResult result = new APIResult(null);
            result.Result = db.Users.ToList();
            return result;
        }

        public APIResult UserUpdate(UserUpdateDto model)
        {
            APIResult result = new APIResult(model);

            var user = db.Users.FirstOrDefault(x => x.Id == model.Id);

            if (user != null)
            {
                user.Email = model.Email;
                user.Name = model.Name;

            }

            db.SaveChanges();

            result.Result = user;
            return result;

        }
        public APIResult UserDelete(int id)
        {
            APIResult result = new APIResult(id);
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            db.Users.Remove(user);
            var x = db.SaveChanges();
            result.Result = x;
            return result;
        }
    }
}

