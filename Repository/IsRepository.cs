using System;
using System.Collections.Generic;
using System.Linq;
using Starship.Core.Context;

namespace Starship.Data.Repository {
    
    public interface IsRepository : IsContext {
        
        void Commit();

        void Delete<T>(T entity) where T : class;

        IQueryable Query(Type type);
        
        T Add<T>(T entity) where T : class;
        
        object Find(Type type, object id);

        IEnumerable<Type> GetTypes();
    }

    public static class IsRepositoryExtensions {
        public static IQueryable<T> Query<T>(this IsRepository context) where T : class {
            return context.Query(typeof (T)) as IQueryable<T>;
        }

        public static T Find<T>(this IsRepository context, object id) where T : class {
            return context.Find(typeof(T), id) as T;
        }
    }
}