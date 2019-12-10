using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WishGrid.Models;

namespace WishGrid.RepositoriesEF
{
    public class CRUDManager<TModel, TPK> where TModel : class
    {
        protected readonly DataContext _dataContext;
        protected readonly DbSet<TModel> _dbSet;
        protected bool _autoFlush;
        private Action actionFlushMode;

        public CRUDManager(DataContext dataContext, DbSet<TModel> models, bool autoFlush = true)
        {
            _dataContext = dataContext;
            _dbSet = models;
            _autoFlush = autoFlush;            
        }

        public DbSet<TModel> Models
        {
            get
            {
                return _dbSet;
            }
        }

        public bool FlushMode
        {
            get
            {
                return _autoFlush;
            }
            set
            {
                if (_autoFlush != value)
                {
                    if (_autoFlush = value)
                    {
                        actionFlushMode = () => Flush();
                    }
                    else
                    {
                        actionFlushMode = () => { };
                    }
                }
            }
        }

        public void Flush()
        {
            _dataContext.SaveChanges();
        }

        public virtual void Insert(TModel model)
        {
            _dataContext.Add(model);
            actionFlushMode();
        }

        public virtual void Update(TModel model)
        {
            _dbSet.Attach(model);
            _dataContext.Entry(model).State = EntityState.Modified;
            actionFlushMode();
        }

        public virtual void Delete(TModel model)
        {
            if (_dataContext.Entry(model).State == EntityState.Detached)
            {
                _dbSet.Attach(model);
            }
            _dbSet.Remove(model);
            actionFlushMode();
        }

        public virtual TModel Select(TPK id)
        {
            return _dbSet.Find(id);
        }

        public virtual List<TModel> SelectAll()
        {
            return _dbSet.ToList();
        }

        public TModel SelectByPK(params object[] keys)
        {
            return _dbSet.Find(keys);
        }

        public virtual IEnumerable<TModel> SelectAllByFilters(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TModel> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public IQueryable<TModel> FindAll(Expression<Func<TModel, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public TModel Find(Expression<Func<TModel, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public bool Exist(Expression<Func<TModel, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
    }
}
