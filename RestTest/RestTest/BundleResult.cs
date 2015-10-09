using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    class BundleResult: Resource
    {
        public OrderResponse orderResponse;
        public DiagnosticReport[] diagnosticReport;
        public Observation[] observation;
        public Practitioner[] practitioner;
    }
}
