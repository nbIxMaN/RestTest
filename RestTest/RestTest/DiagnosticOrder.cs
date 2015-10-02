using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class DiagnosticOrder
    {
        public const string resourceType = "DiagnosticOrder";
        public string id;
        public Link subject;
        public Link orderer;
        public Link encounter;
        public Link[] supportingInformation;
        public Link[] specimen;
        public string status;
<<<<<<< HEAD
        public Item[] item;
=======
        public CodeableConcept[] item;
>>>>>>> 6cd157246ab6b4f56dc9c58857da0c8ba1df6615
    }
}
