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
        public Link subject;
        public Link[] performer;
        public Link requestDetail;
        public Link[] result;
    }
}
