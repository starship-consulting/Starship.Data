using System;
using Starship.Core.Data;

namespace Starship.Data.Services {
    public class SecureDataRepository {

        public SecureDataRepository(IsDataProvider dataProvider) {
            DataProvider = dataProvider;
        }

        /*private Account GetAccount() {
            return Users.GetAccount();
        }*/

        private IsDataProvider DataProvider { get; set; }

        //private readonly AccountManager Users;
    }
}