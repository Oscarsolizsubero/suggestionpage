using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WishGrid.ViewModels.Shared;

namespace WishGrid.IRepositories
{
    public interface IRepositoryCRUD<TViewModel, in TKey> : IRepository where TViewModel : VMBase
    {
        void TInsert(TViewModel viewModel);
        void TUpdate(TViewModel viewModel);
        void TDelete(TViewModel viewModel);
        VMMessage Insert(TViewModel viewModel);
        VMMessage Update(TViewModel viewModel);
        VMMessage Delete(TViewModel viewModel);
        VMMessage Delete(TKey id);        
        TViewModel Select(TKey id);
        TViewModel SelectByPK(params object[] keys);
        List<TViewModel> SelectAll();
    }
}
