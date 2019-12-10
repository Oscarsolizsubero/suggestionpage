using Microsoft.EntityFrameworkCore.Storage;
using System;
using WishGrid.IRepositories;
using WishGrid.Models;

namespace WishGrid.RepositoriesEF
{
    public class REFTransaction : IRepositoryTransaction, IDisposable
    {
        private DataContext _dataContext;
        private IDbContextTransaction _transaction;

        public REFTransaction(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Begin()
        {
            _transaction = _dataContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();                
                _transaction = null;
            }
        }        

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();                
                _transaction = null;
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;                
            }
        }
    }
}
