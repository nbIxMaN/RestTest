using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Identifier
    {
        public string system;
        public string value;
        public Link assigner;
        public Period period; //Период действия. Указывается для паспорта
    }
}
