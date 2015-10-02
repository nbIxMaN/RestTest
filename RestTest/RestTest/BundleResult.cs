using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class BundleResult
    {
        public OrderResponse orederResponse;
        public DiagnosticReport[] diagnosticPeport;
        public Observation[] observation;
        public Practitioner[] practitioner;
    }
}
