using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Starship.Data.Repository {
    
    public class EntityFrameworkCoreDataSet<T> : IsDataSet<T> where T : class {

        internal EntityFrameworkCoreDataSet(DbSet<T> dbSet) {
            DbSet = dbSet;
        }

        public IsDataSet<T> Include(params string[] paths) {
            foreach(var path in paths) {
                DbSet = DbSet.Include(path) as DbSet<T>;
            }

            return this;
        }

        IsDataSet IsDataSet.Include(params string[] paths) {
            return Include(paths);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Expression Expression => ((IQueryable)DbSet).Expression;

        public Type ElementType => ((IQueryable)DbSet).ElementType;

        public IQueryProvider Provider => ((IQueryable)DbSet).Provider;

        public IEnumerator<T> GetEnumerator() {
            return ((IQueryable<T>)DbSet).GetEnumerator();
        }

        private DbSet<T> DbSet { get; set; }
    }
}