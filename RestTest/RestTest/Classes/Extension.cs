using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class Extension
    {
        public string url;
        //содержит или CodeableConcept или Link
        public CodeableConcept valueCodeableConcept;
        public Link valueReference;
    }
}
