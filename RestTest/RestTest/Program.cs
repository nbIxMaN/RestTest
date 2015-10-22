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
    class Program
    {
        public IRestResponse RequestExec(RestSharp.Method method, string url, string s)
        {
            var client = new RestClient() { BaseUrl = new Uri(url) };
            var request = new RestRequest(method) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            request.AddParameter("application/json; charset=utf-8", s, ParameterType.RequestBody);
            return client.Execute(request);
        }

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

            // Coverage co = (new SetData()).SetCoverage("106043a2-6600-4590-bedd-6e26c76a6fed");

            //GetRequest g = new GetRequest
            //{
            //    parameter = new Parameter[]
            //    {
            //        new Parameter
            //        {
            //            name = "TargetCode",
            //            valueString = "123.456.789"
            //        },
            //        new Parameter
            //        {
            //            name = "Barcode",
            //            valueString ="barcode"
            //        }
            //    }
            //};

            var client = new RestClient();
            client.BaseUrl = new Uri("http://192.168.8.93:2223/fhir?_format=json");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;

            // var x = request.JsonSerializer.Serialize(s);
            //string patient = "02255d1f-548c-4b04-9ac2-7c97d3efad1a";

            //Order order = (new SetData()).SetOrder(patient, "Practitioner/131d7d5d-0f21-451d-86ec-27fa3e069e1a");
            //DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient);
            //Encounter enc = (new SetData()).SetEncounter(patient);
            //Condition con = (new SetData()).SetCondition(patient);


            //SetDiagnosticOrder(patient),
            //SetSpecimen(patient),
            //SetEncounter(patient),
            //SetCondition(patient),
            //SetObservation_BundleOrder(),
            //SetPractitioner(),
            //SetCoverage(patient),

            //106043a2-6600-4590-bedd-6e26c76a6fed
            //  Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder,null, enc, con, null, null, null);
            //Bundle b = (new SetData()).SetBundleResult("02255d1f-548c-4b04-9ac2-7c97d3efad1a");
            //request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            //var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            //request.AddParameter("application/json; charset=utf-8", s, ParameterType.RequestBody);
            //var r = client.Execute(request);
        }
    }
}
