using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Cov
    {
        public const string resourseType = "Coverage";
        public Coding type;
        public Identifier identifier;
        public Link subscriber;
    }
}
