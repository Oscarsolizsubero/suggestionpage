using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WishGrid.Models;
using WishGrid.RepositoriesEF;
using WishGrid.ViewModels;

namespace WishGrid.IRepositories
{
    public class REFStatus : IRStatus
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Status> _models;
        private CRUDManager<Status, int> _CRUDManagerStatus;

        public REFStatus(DataContext dataContext)
        {
            _dataContext = dataContext;
            _models = dataContext.Status;
            _CRUDManagerStatus = new CRUDManager<Status, int>(dataContext, dataContext.Status);
        }

        public override IEnumerable<VMStatus> SelectStatusPublic()
        {
            IEnumerable<VMStatus> statuses = _models.Where(p => p.Id == 2 || p.Id == 4).Select(row => new VMStatus()
            {
                Id = row.Id,
                Description = row.Description,
            }).ToList(); ;

            IEnumerable<VMStatus> all = new VMStatus[] { new VMStatus(){
                Id = 0,
                Description = "All" }
            };
            statuses = all.Concat(statuses);
            return statuses;
        }

        public override IEnumerable<VMStatus> SelectStatusPrivate()
        {
            IEnumerable<VMStatus> statuses = _models.Select(row => new VMStatus()
            {
                Id = row.Id,
                Description = row.Description,
            }).ToList();
            IEnumerable<VMStatus> all = new VMStatus[] { new VMStatus(){
                Id = 0,
                Description = "All" }
            };
            statuses = all.Concat(statuses);
            return statuses;
        }
        public override IEnumerable<VMStatus> SelectStatusforEdit(int IdSuggestion)
        {
            Suggestion sug = _dataContext.Suggestion.Include(r=>r.Status).SingleOrDefault(x=>x.Id == IdSuggestion);
            IEnumerable<VMStatus> statuses = null;
            if (sug.Status.Id == 1)
            {
                statuses = _models.Where(p => p.Id == 2 || p.Id == 3).Select(row => new VMStatus()
                {
                    Id = row.Id,
                    Description = row.Description,
                }).ToList(); ;
            }
            if (sug.Status.Id == 2)
            {
                statuses = _models.Where(p => p.Id == 4).Select(row => new VMStatus()
                {
                    Id = row.Id,
                    Description = row.Description,
                }).ToList(); ;
            }
            return statuses;
        }
    }
}
