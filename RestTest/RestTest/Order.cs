using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Order
    {
        public const string resourceType = "Order";
        public Identifier[] identifier;
        public DateTime date;
        public Link subject;
        public Link source;
        public Link target;
        public When when;
        public Link[] detail;
        public string status;
    }
}
