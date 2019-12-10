using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WishGrid.Models.Seed
{
    public class SeedStatus : SeedBaseTemplate<Status>
    {
        private DataContext _DataContext;

        public SeedStatus(DataContext dataContext)
        {
            _DBContext = _DataContext = dataContext;
            _Models = dataContext.Status;
        }

        public override Status GetRecord(CsvReader csvReader)
        {
            return new Status()
            {
                Description = csvReader.GetField("Description"),
            };
        }
    }
}
