using System;
using System.Collections.Generic;
using Starship.Core.Context;
using Starship.Core.Extensions;

namespace Starship.Data.Repository {
    
    public interface IsRepository : IsContext {
        
        void Commit();

        void Delete<T>(T entity) where T : class;

        IsDataSet<T> Get<T>() where T : class;
        
        T Add<T>(T entity) where T : class;
        
        object Find<T>(object id) where T : class;

        IEnumerable<Type> GetTypes();
    }

    public static class IsRepositoryExtensions {

        public static IsDataSet Get(this IsRepository repository, Type type) {
            return repository.InvokeGenericMethod("Get", type) as IsDataSet;
        }

        public static IsDataSet Find(this IsRepository repository, Type type, object id) {
            return repository.InvokeGenericMethod("Find", type, id) as IsDataSet;
        }

        /*public static IQueryable<T> Query<T>(this IsRepository context) where T : class {
            return context.GetDataSet<T>() as IQueryable<T>;
        }*/

        /*public static T Find<T>(this IsRepository context, object id) where T : class {
            return context.Find(typeof(T), id) as T;
        }*/
    }
}