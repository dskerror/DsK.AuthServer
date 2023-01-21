using BlazorWASMCustomAuth.Database;
using BlazorWASMCustomAuth.PagingSortingFiltering;
using BlazorWASMCustomAuth.Security.Shared;
using BlazorWASMCustomAuth.Validations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public partial class SecurityService
    {
        public APIResult UserCreate(UserCreateModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();

            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var UserExists = UserVerifyExistsByUsername(model.Username);
            if (UserExists)
            {
                result.Message = "Username already exists.";
                result.HasError = true;
                return result;
            }

            var EmailExists = UserVerifyExistsByEmail(model.Email);
            if (EmailExists)
            {
                result.Message = "Email already exists.";
                result.HasError = true;
                return result;
            }

            var dbresult = dm.ExecScalarSP("sp_UserCreate",
                "Username", model.Username ?? "",
                "Email", model.Email ?? "",
                "Name", model.FullName ?? "");
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "User Created";

            return result;
        }        
        public UsersGetDTO UsersGet(PagingSortingFilteringRequest request)
        {
            var usersCountResult = dm.ExecScalarSP("sp_UsersCountGet");
            var response = new UsersGetDTO();

            response.TotalRows = (int)usersCountResult.Result;
            if (request != null)
            {
                response.PageSize = request.PageSize;
                response.CurrentPage = request.CurrentPage;
            }

            var list = new List<UserDTO>();
            var UserListDt = dm.ExecDataTableSP("sp_UserList", "PageSize", response.PageSize, "OffSet", response.OffSet());
            foreach (DataRow users in UserListDt.Rows)
            {
                list.Add(new UserDTO()
                {
                    Id = int.Parse(users["Id"].ToString() ?? ""),
                    Name = users["Name"].ToString(),
                    Username = users["Username"].ToString(),
                    Email = users["Email"].ToString()
                });
            }
            response.UserList = list;
            return response;
        }
        public APIResult UserUpdate(UserUpdateModel model)
        {
            APIResult result = new APIResult(model);

            result.ModelValidationResult = model.ValidateModel();
            if (!result.ModelValidationResult.IsValid)
            {
                result.HasError = true;
                result.Message = "See Model Validations";
                return result;
            }

            var userToUpdate = UserGetBy("Id", model.Id.ToString());
            if (userToUpdate == null)
            {
                result.HasError = true;
                result.Message = "Update Failed - Id not found.";
                return result;
            }

            var userWithSameEmail = UserGetBy("Email", model.Email);
            if (userWithSameEmail != null)
            {
                if (userToUpdate.Id != userWithSameEmail.Id)
                {
                    result.Message = "Email already exists.";
                    result.HasError = true;
                    return result;
                }
            }

            DateTime? LockoutEnd = null;

            try
            {
                LockoutEnd = DateTime.Parse(model.LockoutEnd);
            }
            catch (Exception)
            {
                //ignore - ugly
            }


            var dbresult = dm.ExecNonQuerySP("sp_UserUpdate",
                "Id", model.Id,
                "Email", model.Email,
                "Name", model.Name,
                "EmailConfirmed", model.EmailConfirmed,
                "LockoutEnd", LockoutEnd,
                "LockoutEnabled", model.LockoutEnabled,
                "AccessFailedCount", model.AccessFailedCount,
                "TwoFactorEnabled", model.TwoFactorEnabled
                );
            result.Result = dbresult.Result;
            result.HasError = dbresult.HasError;
            result.Exception = dbresult.Exception;
            result.Message = "User Updated";

            return result;
        }
        public APIResult UserDelete(int id)
        {
            APIResult result = new APIResult(id);
            //TODO: cascade delete or disable user?
            //var dbresult = dm.ExecScalarSP("sp_UserDelete", "Id", id);
            //result.Result = dbresult.Result;
            //result.HasError = dbresult.HasError;
            //result.Exception = dbresult.Exception;
            //result.Message = "User Deleted";
            ////TODO: Find if no record was update to give error message in this and other update methods

            return result;
        }
        public bool UserVerifyExistsByUsername(string username)
        {
            var dt = dm.ExecDataTableSP("sp_UserList", "Username", username);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }
        public bool UserVerifyExistsByEmail(string email)
        {
            var dt = dm.ExecDataTableSP("sp_UserList", "Email", email);

            if (dt.Rows.Count == 0)
                return false;

            return true;
        }


        private UserModel UserGetBy(string column, string value)
        {
            UserModel user = new UserModel();

            var UserDt = dm.ExecDataTableSP("sp_UserList", column, value);
            foreach (DataRow dr in UserDt.Rows)
            {
                user.Id = int.Parse(dr["Id"].ToString() ?? "");
                user.Name = dr["Name"].ToString();
                user.Username = dr["Username"].ToString();
                user.Email = dr["Email"].ToString();
                user.Permissions = GetUserPermissions(dr["Username"].ToString());
            }

            if (user.Id == 0)
                return null;
            return user;
        }
    }
}
