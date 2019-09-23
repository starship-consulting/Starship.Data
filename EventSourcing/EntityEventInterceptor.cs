using System;
using System.Threading.Tasks;
using Starship.Core.Security;
using Starship.Data.Entities;
using Starship.Data.Interfaces;

namespace Starship.Data.EventSourcing {
    public class EntityEventInterceptor : IsDataInterceptor {

        public async Task Save(IsSecurityContext context, DocumentEntity document) {
        }

        public async Task Delete(IsSecurityContext context, DocumentEntity document) {
        }
    }
}