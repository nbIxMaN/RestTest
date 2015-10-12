using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class DiagnosticOrder : Resource
    {
        public readonly string resourceType = "DiagnosticOrder";
        public string id;
        public Link subject;
        public Link orderer;
        public Link encounter;
        public Link[] supportingInformation;
        public Link[] specimen;
        public string status;
        public Item[] item;
    }
}
