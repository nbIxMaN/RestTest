using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Coverage : Resource
    {
        public readonly string resourceType = "Coverage";
        public string id;
        
        public Coding type;
        public Identifier identifier;
        public Link subscriber;
    }
}
