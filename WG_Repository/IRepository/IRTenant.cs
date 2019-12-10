using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.IRepositories;
using WishGrid.ViewModels;

namespace WishGrid.IRepositories
{
    public abstract class IRTenant : IRepositoryCRUDTemplate<VMTenant, int>
    {
        public abstract VMTenant select(string tenanturl);
        public abstract VMTenantImage selectImage(string tenanturl);
        public abstract void EditModeration(int tenantId, bool newModStatus);
    }
}
