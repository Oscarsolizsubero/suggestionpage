using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace WishGrid.Models.Seed
{
    public abstract class SeedBaseTemplate<TModel> where TModel : class
    {
        protected DbContext _DBContext;
        protected DbSet<TModel> _Models;

        //protected SeedBaseTemplate(DbContext dbContext)
        //{
        //    //_DBContext = dbContext;
        //    //_Models = _DBContext.Set<TModel>();
        //}

        public abstract TModel GetRecord(CsvReader csvReader);

        public virtual string GetPath()
        {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!path.EndsWith("\\WG_ControllerRS"))
            {
                directoryInfo = directoryInfo.Parent.Parent.Parent;
            }
            path = $"{directoryInfo.Parent.FullName}\\WG_ModelEF\\Data";
            return path;
        }

        public virtual bool Initialize(string fileName, bool hasHeaderRecord = true)
        {
            bool any = _Models.Any();
            if (!any) {
                var csv = new CsvReader(new StreamReader($"{GetPath()}\\{fileName}"));
                csv.Configuration.HasHeaderRecord = hasHeaderRecord;
                using (csv)
                {
                    csv.Read();
                    if (hasHeaderRecord)
                    {
                        csv.ReadHeader();
                    }
                    while (csv.Read())
                    {
                        _Models.Add(GetRecord(csv));
                    }
                    _DBContext.SaveChanges();
                }
            }
            return any;
        }
    }
}
