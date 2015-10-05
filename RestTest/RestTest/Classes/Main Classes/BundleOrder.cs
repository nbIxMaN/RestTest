using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class BundleOrder
    {
        public Order order;
        public Patient patient;
        public Practitioner[] practitioner;
        public DiagnosticOrder[] diagnosticOrder;
        public Specimen[] specimen;
        public Encounter encounter;
        public Condidtion[] condition;
        public Observation[] observation;
        public Coverage[] coverage;
    }
}
