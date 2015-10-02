using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{

    class Coding
    {
        public string system;
        public int version;
        public int code;
    }

    class When
    {
        public Code code;
    }

    class Code
    {
        public Extension[] extension;
        public Coding[] coding;
    }
    class Cov
    {
        public const string resourseType = "Coverage";
        public Coding type;
        public Identifier identifier;
        public Link subscriber;
    }

    class Coverage
    {
        public Cov resourse;
    }

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
    //Циклическая зависимость, стоит подумать ещё
    class Extension
    {
        public string url;
        public Code valueCodeableConcept;
        public Link valueReference;
    }

    //class URLandCODEABLE : Extension
    //{
    //    public string url;
    //    public Code valueCodeableConcept;
    //}

    //class URLandVALUE : Extension
    //{
    //    public string url;
    //    public Link valueReference;
    //}
    //class Item
    //{
    //    public Extension[] extension;
    //    public Coding[] coding;
    //}

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
        public Code[] item;
    }

    class Container
    {
        public Identifier identifier;
        public Code type;
    }
    class Specimen
    {
        public const string resourseType = "Specimen";
        public string id;
        public Code type;
        public Link subject;
        public Link collection;
        public Container container;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Patient p = new Patient()
            {
                address = new Address[]
                {
                    new Address()
                    {
                        Use = "home",
                        text = "МОЖАЙСКАЯ 2-16"
                    }
                },
                identifier = new Identifier[]
                {
                    new Identifier()
                    {
                        system = "urn:oid:1.2.643.2.69.1.2.6",
                        value = "IdPatientMis11111",
                        assigner = new Reference()
                        {
                            reference = "Link/4bcbf113-f99c-41fa-a92d-43f5684fffc5"
                        }
                    }
                },
                birthDate = new DateTime(1980, 08, 31),
                gender = "male",
                name = new HumanName()
                {
                    family = new string[]
                    {
                        "Пушкин"
                    },
                    given = new string[]
                    {
                        "Александр",
                        "Сергеевич"
                    }
                }
            };
            var c = new RestClient();
            c.BaseUrl = new Uri("http://fhir-demo.zdrav.netrika.ru/fhir/Patient");
            var request = new RestRequest(Method.POST);
            var s = request.JsonSerializer.Serialize(p);
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(p);
            //request.AddParameter("application/json", s, RestSharp.ParameterType.RequestBody);
            var r = c.Execute(request);
        }
    }
}
