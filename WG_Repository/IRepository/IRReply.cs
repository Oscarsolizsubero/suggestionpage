using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.ViewModels;

namespace WishGrid.IRepositories
{
    public abstract class IRReply : IRepositoryCRUDTemplate<VMReply, int>
    {
        public abstract override string TInsert(VMReply viewModel);

        public abstract override void TUpdate(VMReply viewModel);

        public abstract IEnumerable<VMReply> SelectAllBySuggestion(int suggestionId);

        public abstract bool AdminorModerator(int userId);

        public abstract bool isFullfilled(int suggestionId);
    }
}
