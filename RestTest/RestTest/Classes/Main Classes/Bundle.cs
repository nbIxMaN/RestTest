using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class Bundle
    {
        public const string resourseType = "Bundle";
        public const string type = "transaction";
        public Link meta; // profile
        public Entry[] entry;
    }
}
