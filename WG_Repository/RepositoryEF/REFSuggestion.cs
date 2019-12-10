using System;
using System.Collections.Generic;
using System.Linq;
using WishGrid.RepositoriesEF;
using WishGrid.ViewModels;
using WishGrid.Models;
using Microsoft.EntityFrameworkCore;
using WishGrid.ViewModels.Shared;
using System.Data.SqlClient;

namespace WishGrid.IRepositories
{
    public class REFSuggestion : IRSuggestion
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Suggestion> _models;
        private readonly DbSet<Votes> _voteModels;
        private CRUDManager<Tenants, int> _CRUDManagerTenant;

        public REFSuggestion(DataContext dataContext)
        {
            _dataContext = dataContext;
            _models = dataContext.Suggestion;
            _voteModels = dataContext.Votes;
            _CRUDManagerTenant = new CRUDManager<Tenants, int>(dataContext, dataContext.Tenant);
        }

        public override string TInsert(VMSuggestion viewModel)
        {
            Suggestion model = new Suggestion()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                QuantityVote = viewModel.QuantityVote,
                CreatedDate = viewModel.CreatedDate,
                UpdatedDate = viewModel.UpdatedDate,
                Author = _dataContext.User.Find(viewModel.IdAuthor),
                Status = _dataContext.Status.Find(viewModel.StatusId)
            };
            _models.Add(model);
            _dataContext.SaveChanges();
            viewModel.Id = model.Id;
            //send email to the moderator of the suggestion created for user

            _dataContext.Database.ExecuteSqlCommand("exec EmailModeratorSuggestion @SuggestioId, @statusId",
                         new SqlParameter("@SuggestioId", viewModel.Id),
                         new SqlParameter("@statusId", model.Status.Id));
            return model.Id.ToString();          
        }

        public override void TUpdate(VMSuggestion viewModel)
        {
            Suggestion model = _models.Find(viewModel.Id);
            model.Title = viewModel.Title;
            model.Description = viewModel.Description;
            model.UpdatedDate = viewModel.UpdatedDate;
            model.StatusId = viewModel.StatusId;
            _models.Attach(model);
            _dataContext.Entry(model).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public override void TDelete(VMSuggestion viewModel)
        {
            Suggestion model = _models.Find(viewModel.Id);
            model.Deleted = true;
            _models.Attach(model);
            _dataContext.Entry(model).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public override VMSuggestion Select(int id)
        {
            Suggestion row = _models.Include(r => r.Author).Include(s=>s.Status).SingleOrDefault(x => x.Id == id);

            if (row != null)
            {
                VMSuggestion vmSuggestion = new VMSuggestion()
                {
                    Id = row.Id,
                    Title = row.Title,
                    Description = row.Description,
                    QuantityVote = row.QuantityVote,
                    CreatedDate = row.CreatedDate,
                    UpdatedDate = row.UpdatedDate,
                    Status = new VMStatus() { Id = row.Status.Id, Description = row.Status.Description },
                    Author = new VMUserSimple() { Id = row.Author.Id, FullName = row.Author.Name + " " + row.Author.LastName }
                };
                return vmSuggestion;
            }
            else
            {
                return null;
            }
        }
        //count de la lista para los Admin y moderadores, te da el count de todas las sugerencias independientemente de los status
        public override int CountPrivate(string tenant, int statusId, string filters)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            int count;
            if (statusId > 0)
            {
                if (filters == null || filters.Length == 0)
                    count = _models.Include(u => u.Author).Include(s => s.Status).Count(row => !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id && (row.StatusId==statusId));
                else
                {
                    filters = $"%{filters}%";
                    count = _models.Include(u => u.Author).Include(s => s.Status)
                        .Count(row => (EF.Functions.Like(row.Title, filters)
                                    || EF.Functions.Like(row.Description, filters)
                                    || EF.Functions.Like(row.Author.UserName, filters))
                                    && !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id
                                    && (row.Status.Id == 2 || row.Status.Id == 4)&&(row.StatusId==statusId));
                }
            }
            else
            {
                if (filters == null || filters.Length == 0)
                    count = _models.Include(u => u.Author).Include(s => s.Status).Count(row => !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id);
                else
                {
                    filters = $"%{filters}%";
                    count = _models.Include(u => u.Author).Include(s => s.Status)
                        .Count(row => (EF.Functions.Like(row.Title, filters)
                                    || EF.Functions.Like(row.Description, filters)
                                    || EF.Functions.Like(row.Author.UserName, filters))
                                    && !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id);
                }
            }
            return count;
        }
        //count de la lista para usuarios comunes, te da el count de las sugerencias en estado 2: requested y 4:fullfilled
        public override int CountPublic(string tenant, int statusId, string filters)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);
            int count;
            if (statusId > 0)
            {
                if (filters == null || filters.Length == 0)
                    count = _models.Include(u => u.Author).Include(s => s.Status).Count(row => !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id && (row.Status.Id == 2 || row.Status.Id == 4) && (row.StatusId == statusId));
                else
                {
                    filters = $"%{filters}%";
                    count = _models.Include(u => u.Author).Include(s => s.Status)
                        .Count(row => (EF.Functions.Like(row.Title, filters)
                                    || EF.Functions.Like(row.Description, filters)
                                    || EF.Functions.Like(row.Author.UserName, filters))
                                    && !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id
                                    && (row.Status.Id == 2 || row.Status.Id == 4) && (row.StatusId == statusId));
                }
            }
            else
            {
                if (filters == null || filters.Length == 0)
                    count = _models.Include(u => u.Author).Include(s => s.Status).Count(row => !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id && (row.Status.Id == 2 || row.Status.Id == 4));
                else
                {
                    filters = $"%{filters}%";
                    count = _models.Include(u => u.Author).Include(s => s.Status)
                        .Count(row => (EF.Functions.Like(row.Title, filters)
                                    || EF.Functions.Like(row.Description, filters)
                                    || EF.Functions.Like(row.Author.UserName, filters))
                                    && !row.Deleted && row.Author.Tenant.Id == tenantencontrado.Id
                                    && (row.Status.Id == 2 || row.Status.Id == 4));
                }
            }
            return count;

        }

        //get list with the attribute bool alreadyvoted
        public override IEnumerable<VMSuggestion> SelectPublic(int pageSize, int pageNumber, string filters, int idUser,string tenant,int statusId)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row=> row.URLOrigin == tenant);

            if (statusId > 0)
            {
                filters = $"%{filters}%";
                IEnumerable<VMSuggestion> query =
                    (from s in _dataContext.Suggestion
                     join u in _dataContext.User on s.AuthorId equals u.Id
                     join v in _dataContext.Votes on s.Id equals v.SuggestionId into sv
                     from leftSV in sv.Where(r => r.UserId == idUser).DefaultIfEmpty()
                     where (EF.Functions.Like(s.Title, filters) || EF.Functions.Like(s.Description, filters)
                             || EF.Functions.Like(u.UserName, filters)) && !s.Deleted && (u.TenantId == tenantencontrado.Id) && (s.Status.Id == 2 || s.Status.Id == 4) && (s.StatusId == statusId)
                     orderby s.QuantityVote descending
                     select new VMSuggestion()
                     {
                         Id = s.Id,
                         Title = s.Title,
                         Description = s.Description,
                         QuantityVote = s.QuantityVote,
                         CreatedDate = s.CreatedDate,
                         UpdatedDate = s.UpdatedDate,
                         Author = new VMUserSimple()
                         {
                             Id = u.Id,
                             FullName = u.Name + " " + u.LastName
                         },
                         IsVoted = leftSV.Suggestion != null,
                         Status = new VMStatus()
                         {
                             Id = s.Status.Id,
                             Description = s.Status.Description
                         }
                     }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return query;
            }
            else
            {
                filters = $"%{filters}%";
                IEnumerable<VMSuggestion> query =
                    (from s in _dataContext.Suggestion
                     join u in _dataContext.User on s.AuthorId equals u.Id
                     join v in _dataContext.Votes on s.Id equals v.SuggestionId into sv
                     from leftSV in sv.Where(r => r.UserId == idUser).DefaultIfEmpty()
                     where (EF.Functions.Like(s.Title, filters) || EF.Functions.Like(s.Description, filters)
                             || EF.Functions.Like(u.UserName, filters)) && !s.Deleted && (u.TenantId == tenantencontrado.Id) && (s.Status.Id == 2 || s.Status.Id == 4)
                     orderby s.QuantityVote descending
                     select new VMSuggestion()
                     {
                         Id = s.Id,
                         Title = s.Title,
                         Description = s.Description,
                         QuantityVote = s.QuantityVote,
                         CreatedDate = s.CreatedDate,
                         UpdatedDate = s.UpdatedDate,
                         Author = new VMUserSimple()
                         {
                             Id = u.Id,
                             FullName = u.Name + " " + u.LastName
                         },
                         IsVoted = leftSV.Suggestion != null,
                         Status = new VMStatus()
                         {
                             Id = s.Status.Id,
                             Description = s.Status.Description
                         }
                     }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return query;
            }
        }
        public override IEnumerable<VMSuggestion> SelectPrivate(int pageSize, int pageNumber, string filters, int idUser, string tenant, int statusId)
        {
            Tenants tenantencontrado = _CRUDManagerTenant.Find(row => row.URLOrigin == tenant);

            if (statusId > 0)
            {
                filters = $"%{filters}%";
                IEnumerable<VMSuggestion> query =
                    (from s in _dataContext.Suggestion
                     join u in _dataContext.User on s.AuthorId equals u.Id
                     join v in _dataContext.Votes on s.Id equals v.SuggestionId into sv
                     from leftSV in sv.Where(r => r.UserId == idUser).DefaultIfEmpty()
                     where (EF.Functions.Like(s.Title, filters) || EF.Functions.Like(s.Description, filters)
                             || EF.Functions.Like(u.UserName, filters)) && !s.Deleted && (u.TenantId == tenantencontrado.Id) && (s.StatusId == statusId)
                     orderby s.QuantityVote descending
                     select new VMSuggestion()
                     {
                         Id = s.Id,
                         Title = s.Title,
                         Description = s.Description,
                         QuantityVote = s.QuantityVote,
                         CreatedDate = s.CreatedDate,
                         UpdatedDate = s.UpdatedDate,
                         Author = new VMUserSimple()
                         {
                             Id = u.Id,
                             FullName = u.Name + " " + u.LastName
                         },
                         IsVoted = leftSV.Suggestion != null,
                         Status = new VMStatus()
                         {
                             Id = s.Status.Id,
                             Description = s.Status.Description
                         }
                     }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return query;
            }
            else
            {
                filters = $"%{filters}%";
                IEnumerable<VMSuggestion> query =
                    (from s in _dataContext.Suggestion
                     join u in _dataContext.User on s.AuthorId equals u.Id
                     join v in _dataContext.Votes on s.Id equals v.SuggestionId into sv
                     from leftSV in sv.Where(r => r.UserId == idUser).DefaultIfEmpty()
                     where (EF.Functions.Like(s.Title, filters) || EF.Functions.Like(s.Description, filters)
                             || EF.Functions.Like(u.UserName, filters)) && !s.Deleted && (u.TenantId == tenantencontrado.Id)
                     orderby s.QuantityVote descending
                     select new VMSuggestion()
                     {
                         Id = s.Id,
                         Title = s.Title,
                         Description = s.Description,
                         QuantityVote = s.QuantityVote,
                         CreatedDate = s.CreatedDate,
                         UpdatedDate = s.UpdatedDate,
                         Author = new VMUserSimple()
                         {
                             Id = u.Id,
                             FullName = u.Name + " " + u.LastName
                         },
                         IsVoted = leftSV.Suggestion != null,
                         Status = new VMStatus()
                         {
                             Id = s.Status.Id,
                             Description = s.Status.Description
                         }
                     }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return query;
            }
        }

        // get suggestion with the attribute bool alreadyvoted
        public override VMSuggestion Select(VMVote votes,string tenant)
        {
            User user = _dataContext.User.Find(votes.UserId);
            Tenants tenantencontrado = _CRUDManagerTenant.Find(t => t.User.Tenant.URLOrigin == tenant);
            Suggestion row = _models.Include(r => r.Author).Include(s=>s.Status).SingleOrDefault(x => x.Id == votes.SuggestionId);

            if (row.Deleted==true)
            { return null; }

            if (row != null && row.Author.TenantId == tenantencontrado.Id)
            {

                VMSuggestion vmSuggestion = new VMSuggestion()
                {
                    Id = row.Id,
                    Title = row.Title,
                    Description = row.Description,
                    QuantityVote = row.QuantityVote,
                    CreatedDate = row.CreatedDate,
                    UpdatedDate = row.UpdatedDate,
                    Author = new VMUserSimple() { Id = row.Author.Id, FullName = row.Author.Name + " " + row.Author.LastName },
                    Status = new VMStatus() { Id = row.Status.Id, Description = row.Status.Description },
                    IsVoted = IsVote(votes)
                };

                if (row.StatusId == 1 || row.StatusId == 3)
                {
                    if (user.RoleId == 2 || user.RoleId == 3)
                    {
                        return vmSuggestion;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return vmSuggestion;
                }
            }
            else
            {
                return null;
            }
        }

        public override VMMessage Vote(VMVote votes)
        {
            Votes vote = new Votes
            {
                SuggestionId = votes.SuggestionId,
                UserId = votes.UserId
            };
            try
            {
                _voteModels.Add(vote);
                Suggestion suggestion = _models.Find(votes.SuggestionId);
                suggestion.QuantityVote++;
                _models.Update(suggestion);
                _dataContext.SaveChanges();
                return new VMMessage(State.Successful);
            }
            catch (Exception ex)
            {
                return new VMMessage(State.Error, ex);
            }
        }

        public override bool IsVote(VMVote vmvote)
        {
            Votes vote = _voteModels.FirstOrDefault(u => u.UserId == vmvote.UserId && u.SuggestionId == vmvote.SuggestionId);

            return vote != null;
        }
        public override bool IsAuthor(VMVote vmvote)
        {
            Suggestion suggestion = _models.FirstOrDefault(u => u.AuthorId == vmvote.UserId && u.Id == vmvote.SuggestionId && u.QuantityVote!=0);
            return suggestion != null;
        }

        public override bool isFullfilled(int suggestionId)
        {
            Suggestion row = _models.Include(r => r.Author).Include(s => s.Status).SingleOrDefault(x => x.Id == suggestionId);
            if (row.Status.Id == 4)
            {
                return true;
            }
            return false;

        }

        public override void VoteDelete(VMVote vote)
        {
            Votes model = _voteModels.FirstOrDefault(u => u.UserId == vote.UserId && u.SuggestionId == vote.SuggestionId);
            if (_dataContext.Entry(model).State == EntityState.Detached)
            {
                _voteModels.Attach(model);
            }
            _voteModels.Remove(model);
            Suggestion suggestion = _models.Find(vote.SuggestionId);
            suggestion.QuantityVote--;
            _models.Update(suggestion);
            _dataContext.SaveChanges();
        }
        public override bool Moderation(int IdAuthor)
        {
            User user = _dataContext.User.Find(IdAuthor);
            Tenants tenantencontrado = _CRUDManagerTenant.Find(t => t.User.Id==user.Id);
            return tenantencontrado.Moderation;
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

        public override VMMessage UpdateStatus(VMSuggestionEditStatus model)
        {
            Suggestion suggestion = _models.Find(model.Id);
            if (model.StatusId == 2 || model.StatusId == 3)
            {
                if (suggestion.StatusId == 1)
                {
                    suggestion.StatusId = model.StatusId;
                    _models.Attach(suggestion);
                    _dataContext.Entry(suggestion).State = EntityState.Modified;
                    _dataContext.SaveChanges();

                    if (model.StatusId == 2)
                    {
                        _dataContext.Database.ExecuteSqlCommand("exec EmailAceptedSuggestion @SuggestioId",
                                                 new SqlParameter("@SuggestioId", model.Id));
                    }                 
                    return new VMMessage(State.Successful);
                }
                else
                {
                    return new VMMessage(State.Error);
                }
            }
            if (model.StatusId == 4)
            {
                if (suggestion.StatusId == 2)
                {
                    suggestion.StatusId = model.StatusId;
                    _models.Attach(suggestion);
                    _dataContext.Entry(suggestion).State = EntityState.Modified;
                    _dataContext.SaveChanges();
                    return new VMMessage(State.Successful);
                }
                else
                {
                    return new VMMessage(State.Error);
                }
            }
            return new VMMessage(State.Error);
        }
    }
}
