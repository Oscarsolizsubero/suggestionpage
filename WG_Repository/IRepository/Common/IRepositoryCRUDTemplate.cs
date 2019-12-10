using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.ViewModels.Shared;

namespace WishGrid.IRepositories
{
    public abstract class IRepositoryCRUDTemplate<TViewModel, TKey> : IRepository where TViewModel : VMBase
    {
        public VMMessage Insert(TViewModel viewModel)
        {
            try
            {
                return new VMMessage(State.Successful, TInsert(viewModel));
            }
            catch (Exception ex)
            {
                return new VMMessage(State.Error, ex);
            }
        }

        public VMMessage Update(TViewModel viewModel)
        {
            try
            {
                TUpdate(viewModel);
                return new VMMessage(State.Successful);
            }
            catch (Exception ex)
            {
                return new VMMessage(State.Error, ex);
            }
        }

        public VMMessage Delete(TViewModel viewModel)
        {
            try
            {
                TDelete(viewModel);
                return new VMMessage(State.Successful);
            }
            catch (Exception ex)
            {
                return new VMMessage(State.Error, ex);
            }
        }

        public VMMessage Delete(TKey id)
        {
            return Delete(Select(id));
        }

        public virtual string TInsert(TViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public virtual void TUpdate(TViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public virtual void TDelete(TViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public virtual TViewModel Select(TKey id)
        {
            throw new NotImplementedException();
        }

        public virtual TViewModel SelectByPK(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public virtual List<TViewModel> SelectAll()
        {
            throw new NotImplementedException();
        }

        public static VMMessage ExecuteAction(Action action)
        {
            try
            {
                action();
                return new VMMessage(State.Successful);
            }
            catch (Exception ex)
            {
                return new VMMessage(State.Error, ex);
            }
        }
    }
}
