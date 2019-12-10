using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WishGrid.Models.Seed
{
    public class SeedTenants : SeedBaseTemplate<Tenants>
    {
        private DataContext _DataContext;

        public SeedTenants(DataContext dataContext)
        {
            _DBContext = _DataContext = dataContext;
            _Models = dataContext.Tenant;
        }

        public override Tenants GetRecord(CsvReader csvReader)
        {
            return new Tenants()
            {
                Address = csvReader.GetField("Address"),
                NameTenants = csvReader.GetField("NameTenants"),
                Nit = csvReader.GetField("Nit"),
                Phone = csvReader.GetField("Phone"),
                Status = csvReader.GetField<bool>("Status"),
                URLOrigin = csvReader.GetField("URLOrigin"),
                Moderation = csvReader.GetField<bool>("Moderation"),
                URLImage = csvReader.GetField("URLImage"),
                URLTenant = csvReader.GetField("URLTenant"),
            };
        }
    }
}
