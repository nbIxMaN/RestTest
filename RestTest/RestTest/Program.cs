using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestTest
{
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
            c.BaseUrl = new Uri("http://fhir.zdrav.netrika.ru/fhir/Patient");
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
