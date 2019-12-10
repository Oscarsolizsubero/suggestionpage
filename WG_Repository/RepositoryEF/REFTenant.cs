using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WishGrid.Models;
using WishGrid.RepositoriesEF;
using WishGrid.ViewModels;

namespace WishGrid.IRepositories
{
    public class REFTenant:IRTenant
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Tenants> _models;
        private CRUDManager<Tenants, int> _CRUDManagerTenant;

        public REFTenant(DataContext dataContext)
        {
            _dataContext = dataContext;
            _models = dataContext.Tenant;
            _CRUDManagerTenant = new CRUDManager<Tenants, int>(dataContext, dataContext.Tenant);
        }

        public override VMTenant select(string tenanturl)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenanturl);
            return new VMTenant
            {
                Id= tenantencontrado.Id,
                Moderation = tenantencontrado.Moderation,
                TenantURL = tenantencontrado.URLOrigin
            };
        }

        public override void EditModeration(int tenantId, bool newModStatus)
        {
            Tenants tenantfound = _models.Find(tenantId);
            tenantfound.Moderation = newModStatus;
            _models.Update(tenantfound);
            _dataContext.SaveChanges();
        }
        public override VMTenantImage selectImage(string tenanturl)
        {
            Tenants tenantenImage = _CRUDManagerTenant.Find(row => row.URLOrigin == tenanturl);
            return new VMTenantImage
            {
                URLImage = tenantenImage.URLImage,
                URLTenant = tenantenImage.URLTenant,
            };
        }
    }
}
