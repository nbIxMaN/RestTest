using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class Bundle
    {
        public readonly string resourceType = "Bundle";
        public readonly string type = "transaction";
        public Link meta; // profile
        public Entry[] entry;
    }
}
