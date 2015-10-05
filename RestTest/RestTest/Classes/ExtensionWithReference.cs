using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    //Циклическая зависимость, стоит подумать ещё
    class ExtensionWithReference : Extension
    {
        public string url;
        public Link valueReference;
    }
}
