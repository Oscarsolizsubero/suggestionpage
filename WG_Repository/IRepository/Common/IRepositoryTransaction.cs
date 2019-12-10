using System;
using System.Collections.Generic;
using System.Text;

namespace WishGrid.IRepositories
{
    public interface IRepositoryTransaction : IRepository
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}
