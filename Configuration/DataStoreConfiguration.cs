using System;
using System.Collections.Generic;
using System.Reflection;
using Starship.Core.Context;
using Starship.Core.Extensions;
using Starship.Core.Reflection;
using Starship.Data.Repository;

namespace Starship.Data.Configuration {

    public class DataStoreConfiguration {

        public DataStoreConfiguration() {
            TypeNameResolver = new TypeNameResolver {
                TypeBinding = GetTypes
            };
        }

        public void RegisterRepositoryType<R>() where R : IsRepository, new() {
            var repository = new R();

            foreach(var type in repository.GetTypes()) {
                DataTypes.Add(type, typeof(R));
            }

            RepositoryTypes.Add(typeof(R));
        }

        public object ResolveId(Type type, object id) {
            var propertyType = type.GetProperty("Id").PropertyType;
            return propertyType == typeof (string) ? id.ToString().Replace("'", "") : id.As(propertyType);
        }

        /*public IODataPathHandler GetODataPathHandler() {
            return new TypeNameODataPathHandler(TypeNameResolver);
        }*/

        public IsRepository GetRepositoryForType(Type type) {
            if(DataTypes.ContainsKey(type)) {
                var repositoryType = DataTypes[type];

                if(ContextResolver != null) {
                    return (IsRepository) ContextResolver.Get(repositoryType, repositoryType.Name);
                }

                return repositoryType.New<IsRepository>();
            }

            return null;
        }

        /*public IEdmModel GetEdmModel() {
            return new ODataModelProvider(GetTypes().ToArray()).GetEdmModel();
        }*/

        public PropertyInfo GetPrimaryKey(Type type) {
            return type.GetProperty("Id");
        }

        public string GetPrimaryKeyName(Type type) {
            return GetPrimaryKey(type).Name;
        }

        /*public IsRepositoryFactory GetProviderForType(Type type) {
            return DataContextProviders.FirstOrDefault(each => each.GetTypes().Contains(type));
        }*/

        public IEnumerable<Type> GetTypes() {
            return DataTypes.Keys;
            //return DataContextProviders.SelectMany(each => each.GetTypes());
        }
        
        public TypeNameResolver TypeNameResolver { get; set; }

        public ContextResolver ContextResolver { get; set; }

        private readonly List<Type> RepositoryTypes = new List<Type>();

        private readonly Dictionary<Type, Type> DataTypes = new Dictionary<Type, Type>();
    }
}