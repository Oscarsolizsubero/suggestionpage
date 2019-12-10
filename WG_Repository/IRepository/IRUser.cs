using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.Models;
using WishGrid.ViewModels;
using WishGrid.ViewModels.Shared;

namespace WishGrid.IRepositories
{
    public abstract class IRUser : IRepositoryCRUDTemplate<VMUser, int>
    {
        //public abstract override string TInsert(VMUser viewModel);
        //public abstract override void TUpdate(VMUser viewModel);
        public abstract override void TDelete(VMUser viewModel);
        public abstract VMUserEncritpted FindByLogin(string userName,string tenant);
        public abstract bool ValidationCreateAccount(string tenant, int token);
        public abstract void ValidationSendEmail(int authroId, string password);
        public abstract VMUserEncritpted FindByIdUser(int idUser);
        public abstract bool VerifyUserName(string email,string URLOrigin);
        public abstract bool VerifyUserNameEmail(string email, string URLOrigin);
        public abstract int Count(string tenant, string filters);
        public abstract IEnumerable<VMUserforList> Select(int pageSize, int pageNumber, string filters, string tenant);
        public abstract void EditUser(VMUserforList user);
        public abstract void UpdatePassword(int idUser, byte[] hash, byte[] salt);
        public abstract void CreateUser(VMUserAdd userAdd, byte[] hash, byte[] salt);
        public abstract void ResetPassword(string tenant, string email, byte[] hash, byte[] salt, string resetPassword);


    }
}
