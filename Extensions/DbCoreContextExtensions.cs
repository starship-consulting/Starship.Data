using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Starship.Data.Extensions {
    public static class DbCoreContextExtensions {

        public static IQueryable Set(this DbContext context, Type T) {
            var method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);
            method = method.MakeGenericMethod(T);
            return method.Invoke(context, null) as IQueryable;
        }

        public static IEnumerable<Type> GetTypes(this DbContext context) {
            return context.Model.GetEntityTypes().Select(each => each.ClrType);
        }
    }
}