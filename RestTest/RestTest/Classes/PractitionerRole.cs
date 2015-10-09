using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class PractitionerRole
    {
        public Link managingOrganization; // Ссылка на Organization
        public CodeableConcept role; 
        public CodeableConcept specialty; 
    }
}
