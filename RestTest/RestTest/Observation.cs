using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Observation
    {
        public CodeableConcept code; //Код теста, для которого передается результат в Observation (1.2.643.2.69.1.1.1.1)
        public string comments;
        public DateTime issued; //Дата-время результата теста. тип instant
        public string status; // Статус ресурса (1.2.643.2.69.1.1.1.47). тип code
        public CodeableConcept method; //Методика исследования
        public Link performer; // Ссылка на Practitioner
        // value[x]
        public CodeableConcept dataAbsentReason; // Причина, по которой результат отсутствует (1.2.643.2.69.1.1.1.38)
        // referenceRange
    }
}
