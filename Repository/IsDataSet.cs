using System;
using System.Linq;

namespace Starship.Data.Repository {
    public interface IsDataSet : IQueryable {
        IsDataSet Include(params string[] paths);
    }

    public interface IsDataSet<out T> : IQueryable<T> {
        IsDataSet<T> Include(params string[] paths);
    }
}