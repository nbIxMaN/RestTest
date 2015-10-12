using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace RestTest
{
    class Encounter : Resource
    {
        public readonly string resourceType = "Encounter";
        public string id;
        public Identifier[] identifier;
        public string status;

        [SerializeAs(Name = "class")]
        public string iHateThisNameClas { get; set; }// class

        public CodeableConcept type;
        public Link patient;
        public CodeableConcept[] reason;
        public Link[] indication;
        public Link serviceProvider;
    }
}
