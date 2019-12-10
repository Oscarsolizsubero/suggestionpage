using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.ViewModels;

namespace WishGrid.IRepositories
{
    public abstract class IRStatus : IRepositoryCRUDTemplate<VMStatus, int>
    {
        public abstract IEnumerable<VMStatus> SelectStatusPublic();

        public abstract IEnumerable<VMStatus> SelectStatusPrivate();

        public abstract IEnumerable<VMStatus> SelectStatusforEdit(int IdSuggestion);
    }
}
