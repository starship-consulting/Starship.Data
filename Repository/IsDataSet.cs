using System;
using System.Linq;

namespace Starship.Data.Repository {
    public interface IsDataSet : IQueryable {
        IsDataSet Include(params string[] paths);
    }

    public interface IsDataSet<out T> : IsDataSet, IQueryable<T> {
        new IsDataSet<T> Include(params string[] paths);
    }
}