using System;
using System.Collections.Generic;

namespace Starship.Data.Repository {
    public interface IsRepositoryFactory {
        IEnumerable<Type> GetTypes();
        IsRepository GetRepository();
    }
}