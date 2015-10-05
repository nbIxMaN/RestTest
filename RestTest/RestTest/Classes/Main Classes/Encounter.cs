using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class Encounter
    {
        public Identifier identifier;
        public string status;
        public string clas; // class
        public CodeableConcept type;
        public Link patient;
        public CodeableConcept reason;
        public Link[] indication;
        public Link serviceProvider;
    }
}
