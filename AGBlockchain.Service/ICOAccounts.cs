using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGBlockchain.Service
{
    public class ICOAccount
    {
        public string Address { get; init; }
        public string Name { get; init; }
        public double Balance { get; init; }

        public static List<ICOAccount> GetICOAccounts()
        {
            return new()
            {
               new()
               {
                   //secret 54096847217171982311628249058628640564832121920247941216028463084843494061241
                   //Name = "Alice"
                   Address = "oI6vbxqQU0vaOQOV/GNtXul4uISE3HiLl7uAvNns98I=",
                   Balance = 100
               },
                new()
                {
                    //secret 22204179903705801691420004542051501762135789649178748678176085086174783232300
                    //Name = "Bob"
                    Address = "mKY0VGPsAhvvoXrQfju6L19bxlvSRE3KIx695b0HiSg=",
                    Balance = 100
                }
            };
        }
    }
}
