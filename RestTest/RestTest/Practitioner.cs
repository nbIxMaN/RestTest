using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Practitioner
    {
        public Identifier[] identifier;
        public HumanName name;
        public Link organization; // Ссылка на Organization
        public CodeableConcept role; // (1.2.643.5.1.13.2.1.1.607)
        public CodeableConcept specialty; // system = 1.2.643.5.1.13.2.1.1.181; ???
        //specialty.Coding.system = "http://netrika.ru/practitioner-speciality"; 
    }
}
