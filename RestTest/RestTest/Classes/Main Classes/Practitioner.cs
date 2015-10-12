using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
    class Practitioner : Resource
    {
        public readonly string resourсeType = "Practitioner";
        public string id;
        public Identifier identifier;
        public HumanName name;
        public PractitionerRole practitionerRole;
    }
}
