using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Starship.Data.Repository {

    public class EntityFrameworkDataSet : IsDataSet {

        internal EntityFrameworkDataSet(DbSet dbSet) {
            DbSet = dbSet;
        }

        public IsDataSet Include(params string[] paths) {

            foreach(var path in paths) {
                DbSet = DbSet.Include(path);
            }

            return this;
        }
        
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Expression Expression => ((IQueryable)DbSet).Expression;

        public Type ElementType => ((IQueryable)DbSet).ElementType;

        public IQueryProvider Provider => ((IQueryable)DbSet).Provider;

        public IEnumerator GetEnumerator() {
            return ((IQueryable)DbSet).GetEnumerator();
        }

        private DbQuery DbSet { get; set; }
    }

    public class EntityFrameworkDataSet<T> : IsDataSet<T> where T : class {

        internal EntityFrameworkDataSet(DbSet<T> dbSet) {
            DbSet = dbSet;
        }

        public IsDataSet<T> Include(params string[] paths) {
            foreach(var path in paths) {
                DbSet = DbSet.Include(path);
            }

            return this;
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

        private DbQuery<T> DbSet { get; set; }
    }
}