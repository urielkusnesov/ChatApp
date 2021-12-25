using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepositoryService
    {
        IList<TEntidad> List<TEntidad>(Expression<Func<TEntidad, bool>> filter = null) where TEntidad : class;

        bool Exists<TEntidad>(Expression<Func<TEntidad, bool>> filter) where TEntidad : class;

        TEntidad Add<TEntidad>(TEntidad entidad) where TEntidad : class;

        int SaveChanges();

    }
}
