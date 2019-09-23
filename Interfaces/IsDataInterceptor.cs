using System.Threading.Tasks;
using Starship.Core.Security;
using Starship.Data.Entities;

namespace Starship.Data.Interfaces {
    public interface IsDataInterceptor {

        Task Save(IsSecurityContext context, DocumentEntity document);

        Task Delete(IsSecurityContext context, DocumentEntity document);
    }
}