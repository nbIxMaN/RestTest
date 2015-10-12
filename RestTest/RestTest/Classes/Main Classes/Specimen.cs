using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Specimen : Resource
    {
        public readonly string resourсeType = "Specimen";
        public string id;
        
        public CodeableConcept type;
        public Link subject;
        public Collection collection;
        public Container container;
    }
}
