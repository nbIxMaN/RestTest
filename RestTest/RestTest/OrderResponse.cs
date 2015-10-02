using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class OrderResponse
    {
        public Link request;
        public Link who;
        public Link[] fulfillment;
    }
}
