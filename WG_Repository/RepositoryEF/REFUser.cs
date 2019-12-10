using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WishGrid.IRepositories;
using WishGrid.RepositoriesEF;
using WishGrid.ViewModels;
using WishGrid.Models;
using Microsoft.EntityFrameworkCore;
using WishGrid.ViewModels.Shared;
using System.Data.SqlClient;

namespace WishGrid.RepositoriesEF
{
    public class REFUser : IRUser
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<User> _models;
        private CRUDManager<User, int> _CRUDManager;
        private CRUDManager<Tenants, int> _CRUDManagerTenant;
        public REFUser() { }
        public REFUser(DataContext dataContext)
        {
            _dataContext = dataContext;
            _models = dataContext.User;
            _CRUDManager = new CRUDManager<User, int>(dataContext, dataContext.User);
            _CRUDManagerTenant = new CRUDManager<Tenants, int>(dataContext, dataContext.Tenant);
        }
        /*CREATE USER*/
        public override void CreateUser(VMUserAdd userAdd, byte[] hash, byte[] salt)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == userAdd.Tenant);
            User model = new User()
            {
                Name = userAdd.Name,
                LastName = userAdd.LastName,
                Email = userAdd.Email,
                UserName = userAdd.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                TenantId = tenantencontrado.Id,
                RoleId = 4,
                Validation = false,
            };                
            _models.Add(model);
            _dataContext.SaveChanges();
            userAdd.Id = model.Id;
            string token = Encode(model.Id + "." + model.Name );
            _dataContext.Database.ExecuteSqlCommand("exec EmailCreateUserAndConfirmation @idUsuario,@idTenant,@token",
                                               new SqlParameter("@idUsuario", model.Id),
                                               new SqlParameter("@idTenant", model.TenantId),
                                               new SqlParameter("@token", token));
        }
        public static string Encode(string textToken)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(textToken);
            return Convert.ToBase64String(plainTextBytes);
        }
        /*CHANGE PASSWORD */
        public override void UpdatePassword(int idUser, byte[] hash, byte[] salt)
        {
            User model = _models.Find(idUser);
            model.PasswordHash = hash;
            model.PasswordSalt = salt;
            _models.Update(model);
            _dataContext.SaveChanges();
        }
        /*RESET PASSWORD*/
        public override void ResetPassword(string tenant, string email, byte[] hash, byte[] salt, string resetPassword)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            User user = _CRUDManager.Find(row => row.Email == email && row.TenantId == tenantencontrado.Id);

            User model = _models.Find(user.Id);
            model.UserName = user.UserName;
            model.Name = user.Name;
            model.LastName = user.LastName;
            model.PasswordHash = hash;
            model.PasswordSalt = salt;
            model.Validation = false;
            _models.Update(model);
            _dataContext.SaveChanges();
            string token = Encode(Convert.ToString(user.Id) + "." + resetPassword + ".true");
            //Send mail to the user with the new password
            _dataContext.Database.ExecuteSqlCommand("exec EmailResetPassword @AuthorId, @Email, @Token",
                          new SqlParameter("@AuthorId", user.Id),
                          new SqlParameter("@Email", user.Email),
                          new SqlParameter("@Token", token));
        }

        public override void TDelete(VMUser viewModel)
        {
            _CRUDManager.Delete(new User() { Id = viewModel.Id });
        }

        public override VMUserEncritpted FindByLogin(string userName,string tenant)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            User user = _CRUDManager.Find(row => row.UserName == userName && row.TenantId == tenantencontrado.Id && row.Validation == true);
            if(user !=null)
            {
                VMUser vmUser = new VMUser()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    IdTenant = user.TenantId,
                    RoleId = user.RoleId
                };
                VMUserEncritpted vmUserEncritpted = new VMUserEncritpted(vmUser)
                {
                    PasswordHash = user.PasswordHash,
                    PasswordSalt = user.PasswordSalt
                };
                return vmUserEncritpted;
            }
            return null;
        }

        public override VMUserEncritpted FindByIdUser(int idUser)
        {
            User user = _CRUDManager.Find(row => row.Id == idUser);
            VMUser vmUser = new VMUser()
            {
                Id = user.Id,
                Name = user.UserName,
                IdTenant = user.TenantId,
                RoleId = user.RoleId
            };
            VMUserEncritpted vmUserEncritpted = new VMUserEncritpted(vmUser)
            {
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };
            return vmUserEncritpted;
        }
        public override bool VerifyUserName(string username, string URLOrigin)
        {
            //The string username is equal to the email, due to the change made that the username will be your own email account
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == URLOrigin);
            User user = _CRUDManager.Find(row => row.UserName == username && row.TenantId == tenantencontrado.Id);
            if (user == null)
            {
                return true;
            }
            return false;
        }
        public override bool VerifyUserNameEmail(string email, string URLOrigin)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == URLOrigin);
            User user = _CRUDManager.Find(row => row.Email == email && row.TenantId == tenantencontrado.Id);
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public override int Count(string tenant, string filters)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            int count;
            if (filters == null || filters.Length == 0)
                count = _models.Count(row => row.TenantId == tenantencontrado.Id && (row.RoleId == 3 || row.RoleId == 4));
            else
            {
                filters = $"%{filters}%";
                count = _models
                    .Count(row => (EF.Functions.Like(row.Name, filters)
                                || EF.Functions.Like(row.LastName, filters)
                                || EF.Functions.Like(row.Email, filters)
                                || EF.Functions.Like(row.UserName, filters))
                                && row.TenantId == tenantencontrado.Id && (row.RoleId == 3 || row.RoleId == 4));
            }
            return count;
        }

        //LIST USERS FILTERED AND ONLY THE QUANTITY CALLED
        public override IEnumerable<VMUserforList> Select(int pageSize, int pageNumber, string filters, string tenant)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            filters = $"%{filters}%";
            IEnumerable<VMUserforList> query =
                (from u in _dataContext.User where (EF.Functions.Like(u.Name, filters) || EF.Functions.Like(u.LastName, filters) || EF.Functions.Like(u.Email, filters)
                         || EF.Functions.Like(u.UserName, filters)) && (u.TenantId == tenantencontrado.Id) && (u.RoleId == 3 || u.RoleId == 4)
                 orderby u.Name ascending
                 select new VMUserforList()
                 {
                     Id = u.Id,
                     Username = u.UserName,
                     FullName = $"{u.Name} {u.LastName}",
                     Email = u.Email,
                     Moderator = u.RoleId == 3,
                 }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return query;
        }

        public override void EditUser(VMUserforList user)
        {
            User userfound = _models.Find(user.Id);
            userfound.UserName = user.Username;
            if (user.Moderator)
            {
                userfound.RoleId = 3;
                _dataContext.Database.ExecuteSqlCommand("exec EmailDesignedModerator @AuthorId",
                           new SqlParameter("@AuthorId", user.Id));
            }
            else
            {
                userfound.RoleId = 4;
            }
            userfound.Email = user.Email;
            _models.Update(userfound);
            _dataContext.SaveChanges();
        }
        public override bool ValidationCreateAccount(string tenant, int tokenId)
        {
            int userId = Convert.ToInt32(tokenId);
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            User user = _CRUDManager.Find(row => row.Id == userId && row.TenantId == tenantencontrado.Id && row.Validation == false);
            if (user != null)
            {
                user.Validation = true;
                _models.Update(user);
                _dataContext.SaveChanges();
                return true;
            }
        return false;
        }

        public override void ValidationSendEmail(int authroId, string password)
        {
            _dataContext.Database.ExecuteSqlCommand("exec EmailPasswordReset @AuthorId, @ResetPassword",
                                     new SqlParameter("@AuthorId", authroId),
                                     new SqlParameter("@ResetPassword", password));
        }
    }
}
