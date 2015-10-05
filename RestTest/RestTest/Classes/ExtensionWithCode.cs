using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    //Циклическая зависимость, стоит подумать ещё
    class ExtensionWithCode : Extension
    {
        public string url;
        public CodeableConcept valueCodeableConcept;
    }
}
