using System.Threading.Tasks;
using Starship.Core.Security;
using Starship.Data.Entities;
using Starship.Data.Utilities;

namespace Starship.Data.Interfaces {
    public interface IsDataInterceptor {

        Task<DocumentChangeset> Save(IsSecurityContext context, DocumentEntity document);

        Task Delete(IsSecurityContext context, DocumentEntity document);
    }
}