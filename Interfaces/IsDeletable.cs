using System;
using System.Collections.Generic;
using System.Linq;

namespace Starship.Data.Interfaces {

    public interface IsDeletable {
        DateTime? ValidUntil { get; set; }
        void Delete();
    }

    public static class IsDeletableExtensions {

        public static void Invalidate(this IsDeletable entity) {
            entity.ValidUntil = DateTime.UtcNow;
        }

        public static IQueryable<T> WithValid<T>(this IQueryable<T> queryable) where T : class, IsDeletable {
            return queryable.Where(each => each.ValidUntil == null || each.ValidUntil >= DateTime.UtcNow);
        }

        public static List<T> GetValid<T>(this List<T> queryable) where T : class, IsDeletable {
            return queryable.Where(each => each.ValidUntil == null || each.ValidUntil >= DateTime.UtcNow).ToList();
        }
    }
}