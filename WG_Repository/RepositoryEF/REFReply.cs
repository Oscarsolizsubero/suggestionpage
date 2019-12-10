using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WishGrid.Models;
using WishGrid.ViewModels;

namespace WishGrid.IRepositories
{
    public class REFReply :IRReply
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Reply> _models;

        public REFReply(DataContext dataContext)
        {
            _dataContext = dataContext;
            _models = dataContext.Replies;
        }

        public override string TInsert(VMReply viewModel)
        {
            Reply model = new Reply()
            {
                Description = viewModel.Description,
                CreatedDate = viewModel.CreatedDate,
                UpdatedDate = viewModel.UpdatedDate,
                Author = _dataContext.User.Find(viewModel.IdAuthor),
                Suggestion = _dataContext.Suggestion.Find(viewModel.IdSuggestion)
            };
            _models.Add(model);
            _dataContext.SaveChanges();
            viewModel.Id = model.Id;
            viewModel.Author.FullName = model.Author.UserName;
            return model.Id.ToString();
        }

        public override void TUpdate(VMReply viewModel)
        {
            Reply model = _models.Find(viewModel.Id);
            model.Description = viewModel.Description;
            model.UpdatedDate = viewModel.UpdatedDate;
            _models.Attach(model);
            _dataContext.Entry(model).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public override IEnumerable<VMReply> SelectAllBySuggestion(int suggestionId)
        {
            return _models.Where(p => p.Suggestion.Id == suggestionId).Include(x => x.Author).Include(s => s.Suggestion).Select(row => new VMReply()
            {
                Id = row.Id,
                Description = row.Description,
                CreatedDate = row.CreatedDate,
                UpdatedDate = row.UpdatedDate,
                Author = new VMUserSimple() { Id = row.Author.Id, FullName = row.Author.UserName },
                IdSuggestion = row.Suggestion.Id
            }).ToList();
        }

        public override bool AdminorModerator(int IdUser)
        {
            User user = _dataContext.User.Find(IdUser);
            if (user.RoleId == 2 || user.RoleId == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool isFullfilled(int suggestionId)
        {
            Suggestion row = _dataContext.Suggestion.Include(r => r.Author).Include(s => s.Status).SingleOrDefault(x => x.Id == suggestionId);
            if (row.Status.Id == 4)
            {
                return true;
            }
            return false;

        }
    }
}
