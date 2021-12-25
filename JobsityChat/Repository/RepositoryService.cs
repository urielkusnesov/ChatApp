using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly DbContext context;

        public RepositoryService(DbContext context)
        {
            this.context = context;
        }

        private IDbSet<TEntidad> Set<TEntidad>() where TEntidad : class
        {
            return context.Set<TEntidad>();
        }

        public IList<TEntidad> List<TEntidad>(Expression<Func<TEntidad, bool>> filter = null) where TEntidad : class
        {
            IQueryable<TEntidad> result = context.Set<TEntidad>();
            if (filter != null)
            {
                result = result.Where(filter);
            }
            return result.ToList();
        }

        public bool Exists<TEntidad>(Expression<Func<TEntidad, bool>> filter) where TEntidad : class
        {
            return Set<TEntidad>().Where(filter).Any();
        }

        public TEntidad Add<TEntidad>(TEntidad entidad) where TEntidad : class
        {
            return Set<TEntidad>().Add(entidad);
        }

        public int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
