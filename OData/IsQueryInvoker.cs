using System.Collections.Generic;

namespace Starship.Data.OData {
    public interface IsQueryInvoker {

        List<T> Get<T>(ODataQuery query);
    }
}