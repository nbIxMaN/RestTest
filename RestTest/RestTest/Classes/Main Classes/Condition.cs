using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class Condition
    {
        public const string resourseType = "Condition";
        public string id;
        public Identifier identifier;
        public Link subject;
        public DateTime dateAsserted;
        public CodeableConcept code;
        public CodeableConcept category;
        public string clinicalStatus;
        public string notes;
        public DueTo dueTo; // Сопутствующее заболевание/осложнение 
    }
}
