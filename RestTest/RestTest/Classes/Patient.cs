using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Patient
    {
        public string resourceType = "Patient";
        public Identifier[] identifier;
        public HumanName name;
        public string gender;
        public DateTime birthDate;
        public Address[] address;
    }
}
