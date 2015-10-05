using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class DiagnosticReport
    {
        public CodeableConcept name; //Код услуги результата (1.2.643.2.69.1.1.1.31)
        public string status; // В сервисе предполагается получать только утвержденные результаты по услуге (1.2.643.2.69.1.1.1.46)
        public DateTime issued; //Дата-время утверждения результата по услуге
        public Link subject; // Ссылка на Patient
        public Link performer; // Ссылка на Practitioner
        public Link requestDetail; // Ссылка на DiagnosticOrder
        public Link[] result; // Ссылка на Observation
        public string conclusion; // Текст заключения по услуге
        public PresentedForm presentedForm;
    }
}
