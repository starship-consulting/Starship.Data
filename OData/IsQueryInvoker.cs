using System.Collections.Generic;
using System.Threading.Tasks;

namespace Starship.Data.OData {
    public interface IsQueryInvoker {

        Task<List<T>> GetAsync<T>(ODataQuery query);

        List<T> Get<T>(ODataQuery query);
    }
}