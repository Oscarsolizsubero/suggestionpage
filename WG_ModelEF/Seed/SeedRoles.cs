using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WishGrid.Models.Seed
{
    public class SeedRoles : SeedBaseTemplate<Role>
    {
        private DataContext _DataContext;

        public SeedRoles(DataContext dataContext)
        {
            _DBContext = _DataContext = dataContext;
            _Models = dataContext.Role;
        }

        public override Role GetRecord(CsvReader csvReader)
        {
            return new Role()
            {
                RoleName = csvReader.GetField("RoleName")
            };
        }
    }
}
