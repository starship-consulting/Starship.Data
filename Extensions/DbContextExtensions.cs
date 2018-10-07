using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Starship.Core.Extensions;
using Starship.Core.Utility;

namespace Starship.Data.Extensions {
    public static class DbContextExtensions {
        public static List<Type> GetConfiguredTypes(this DbModelBuilder builder) {
            return builder.Configurations.GetFieldValue<object>("_modelConfiguration").GetPropertyValue<IEnumerable<Type>>("ConfiguredTypes").ToList();
        }

        public static string GetPrimaryKeyName(this DbContext context, Type type) {
            var table = GetTable(context, type.Name);

            if (table == null) {
                return type.Name + "Id";
            }

            return table.ElementType.KeyMembers.Select(each => each.Name).DefaultIfEmpty().FirstOrDefault();
        }

        public static EdmMember GetPrimaryKey(this DbContext context, Type type) {
            return GetTable(context, type.Name).ElementType.KeyMembers.FirstOrDefault();
        }

        public static bool Contains<T>(this DbContext context, T entity) {
            return context.Set(entity.GetType()).Local.Contains(entity);
        }

        public static List<object> GetEntities(this DbContext context, EntityState state = ~EntityState.Detached) {
            return ((IObjectContextAdapter) context).ObjectContext.ObjectStateManager.GetObjectStateEntries(state).Select(each => each.Entity).ToList();
        }

        public static IQueryable<T> EmptyQuery<T>(this DbContext context) {
            return new List<T>().AsQueryable();
        }

        public static void Detach(this DbContext context, object entity) {
            ((IObjectContextAdapter) context).ObjectContext.Detach(entity);
        }

        public static bool IsSqlMapped(this DbContext context, Type type) {
            return ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace).Any(e => e.Name == type.Name);
        }

        public static void TruncateTable(this DbContext context, Type type) {
            var table = GetTableName(context, type);
            context.Database.ExecuteSqlCommand("truncate table " + table);
        }

        public static void ClearAllTables(this DbContext context) {
            var tables = context.GetTableNames();

            var sb = new StringBuilder();

            foreach (var table in tables) {
                sb.AppendLine("delete from " + table);
            }

            context.Database.ExecuteSqlCommand(sb.ToString());
        }

        public static IEnumerable<EntitySet> GetEntitySets(this DbContext context) {
            var metadata = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;

            return metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Select(each => each.EntityTypeMappings.First().Fragments.First().StoreEntitySet);
        }

        public static IEnumerable<EntityType> GetEntityTypes(this DbContext context) {
            var objectContext = ((IObjectContextAdapter) context).ObjectContext;
            return objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
        }

        public static IEnumerable<Type> GetTypes(this DbContext context) {
            var objectContext = ((IObjectContextAdapter) context).ObjectContext;
            var collection = objectContext.MetadataWorkspace.GetItemCollection(DataSpace.OSpace) as ObjectItemCollection;
            return collection.GetItems<EntityType>().Select(each => collection.GetClrType(each));
        }

        public static IEnumerable<string> GetTypeNames(this DbContext context) {
            return context.GetEntityTypes().Select(each => each.Name);
        }

        public static EntitySet GetTable(this DbContext context, string name) {
            name = name.ToLower();
            return context.GetEntitySets().FirstOrDefault(each => (each.MetadataProperties["Table"].Value ?? each.Name).ToString().ToLower() == name);
        }

        public static Type GetTableType(this DbContext context, string name) {
            var table = context.GetTable(name);
            return table != null ? ReflectionCache.Lookup(table.Name) : null;
        }

        public static IEnumerable<string> GetTableNames(this DbContext context) {
            return context.GetEntitySets()
                .Select(each => each.MetadataProperties["Table"].Value ?? each.Name)
                .Select(each => each.ToString());
        }

        public static string GetTableName(this DbContext context, Type type) {
            var metadata = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection) metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return (string) table.MetadataProperties["Table"].Value ?? table.Name;
        }
    }
}