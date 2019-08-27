using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using Starship.Data.Extensions;

namespace Starship.Data.Repository {
    public class EntityFrameworkRepository<R> : IsRepository where R : DbContext, new() {
        
        public EntityFrameworkRepository() {
            Id = Guid.NewGuid();
            Context = new R();
        }
        
        public void Dispose() {
            Context.Dispose();
            Context = null;
        }

        public void Commit() {
            Context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class {
            Context.Set(entity.GetType()).Remove(entity);
        }

        public IsDataSet<T> Get<T>() where T : class {
            return new EntityFrameworkDataSet<T>(Context.Set<T>());
        }

        /*public IQueryable Query(Type type) {
            return Context.Set(type);
        }*/

        public T Add<T>(T entity) where T : class {
            Context.Set<T>().Add(entity);
            return entity;
        }
        
        public object Find<T>(object id) where T : class {
            return Context.Set<T>().Find(id);
        }

        public TProperty Load<T, TProperty>(T entity, Expression<Func<T, TProperty>> property) where T : class where TProperty : class {
            throw new NotImplementedException();
        }
        
        public IEnumerable<Type> GetTypes() {
            return Context.GetTypes();
        }

        public string GetId() {
            return Id.ToString();
        }

        public void SetId(object value) {
            Id = (Guid)value;
        }

        public Guid Id { get; set; }

        private DbContext Context { get; set; }
    }
}