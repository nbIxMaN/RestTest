using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    //Циклическая зависимость, стоит подумать ещё
    class Extension
    {
        public string url;
        public Code valueCodeableConcept;
        public Link valueReference;
    }
}
