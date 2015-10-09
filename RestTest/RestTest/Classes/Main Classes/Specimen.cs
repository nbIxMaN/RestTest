using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Specimen
    {
        public const string resourseType = "Specimen";
        public string id;
        public CodeableConcept type;
        public Link subject;
        public Collection collection;
        public Container container;
    }
}
