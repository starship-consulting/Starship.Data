using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Starship.Core.Extensions;
using Starship.Data.Configuration;
using Starship.Data.Repository;

namespace Starship.Data {
    public static class DataStore {

        static DataStore() {
            Configuration = new DataStoreConfiguration();
        }
        
        public static IQueryable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class {
            return Configuration.GetRepositoryForType(typeof(T)).Get<T>().Where(criteria);
        }

        public static IQueryable<T> Get<T>() where T : class {
            return Configuration.GetRepositoryForType(typeof(T)).Get<T>();
        }

        public static IQueryable Get(string typeName) {
            return Get(FindType(typeName));
        }

        public static IsDataSet Get(Type type) {
            return Configuration.GetRepositoryForType(type).Get(type);
        }

        public static T Add<T>(params T[] entities) where T : class {
            var instance = Configuration.GetRepositoryForType(typeof(T));

            foreach (var entity in entities) {
                instance.Add(entity);
            }

            return entities.FirstOrDefault();
        }

        public static object Save(string typeName, object data) {
            var type = FindType(typeName);
            var entity = JObject.FromObject(data).ToObject(type);

            var context = Configuration.GetRepositoryForType(type);
            context.InvokeGenericMethod("Add", type, entity);
            context.Commit();

            return entity;
        }

        public static void Delete(string typeName, object id) {
            var type = FindType(typeName);
            var context = Configuration.GetRepositoryForType(type);
            var entity = Find(type, id);

            if (entity != null) {
                context.InvokeGenericMethod("Delete", type, entity);
                context.Commit();
            }
        }

        public static T Find<T>(object id) where T : class {
            return (T) Find(typeof(T), id);
        }

        public static object Find(string typeName, object id) {
            return Find(FindType(typeName), id);
        }

        public static object Find(Type type, object id) {
            return Configuration.GetRepositoryForType(type).Find(type, Configuration.ResolveId(type, id));
        }

        public static Type FindType(string typeName) {
            return Configuration.TypeNameResolver.FindType(typeName);
        }

        public static PropertyInfo GetPrimaryKey(Type elementType) {
            return Configuration.GetPrimaryKey(elementType);
        }

        public static string GetPrimaryKeyName(Type elementType) {
            return Configuration.GetPrimaryKeyName(elementType);
        }

        public static IEnumerable<Type> GetTypes() {
            return Configuration.GetTypes();
        }

        public static DataStoreConfiguration Configuration { get; set; }
    }
}