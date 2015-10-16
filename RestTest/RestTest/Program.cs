using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Serializers;
using System.IO;
using Hl7.Fhir.Model;
using RestSharp;

namespace RestTest
{
        /// <summary>
        /// Default JSON serializer for request bodies
        /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
        /// </summary>

    class Program
    {
        static void Main(string[] args)
        {
            //Patient p = new Patient()
            //{
            //    address = new Address[]
            //    {
            //        new Address()
            //        {
            //            Use = "home",
            //            text = "МОЖАЙСКАЯ 2-16"
            //        }
            //    },
            //    identifier = new Identifier[]
            //    {
            //        new Identifier()
            //        {
            //            system = "urn:oid:1.2.643.2.69.1.2.6",
            //            value = "IdPatientMis11111",
            //            assigner = new Reference()
            //            {
            //                reference = "Link/4bcbf113-f99c-41fa-a92d-43f5684fffc5"
            //            }
            //        }
            //    },
            //    birthDate = new DateTime(1980, 08, 31),
            //    gender = "male",
            //    name = new HumanName()
            //    {
            //        family = new string[]
            //        {
            //            "Пушкин"
            //        },
            //        given = new string[]
            //        {
            //            "Александр",
            //            "Сергеевич"
            //        }
            //    }
            //};
            Patient pa = (new SetData()).SetPatient();
            //pa.address = null;
            //pa.name = null;
            //pa.resourceType = null;
           // Coverage co = (new SetData()).SetCoverage("106043a2-6600-4590-bedd-6e26c76a6fed");


            GetRequest g = new GetRequest
            {
                parameter = new Parameter[]
                {
                    new Parameter
                    {
                        name = "TargetCode",
                        valueString = "123.456.789"
                    },
                    new Parameter
                    {
                        name = "Barcode",
                        valueString ="barcode"
                    }
                }
            };




            var client = new RestClient();
            client.BaseUrl = new Uri("http://fhir.zdrav.netrika.ru/fhir/$getorder?_format=json");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            //var s = new Encounter
            //{
            //    iHateThisNameClas = "sdfsdf"
            //};
         //   var x = request.JsonSerializer.Serialize(s);
            Bundle b = (new SetData()).SetBundleOrder("106043a2-6600-4590-bedd-6e26c76a6fed");
           // var s = request.JsonSerializer.Serialize(b);
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            request.AddParameter("application/json; charset=utf-8", s, ParameterType.RequestBody);
            //request.AddParameter("application/json", s, RestSharp.ParameterType.RequestBody);
            var r = client.Execute(request);
        }
    }
}
