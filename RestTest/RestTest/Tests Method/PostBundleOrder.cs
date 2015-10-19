using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers;
using System.IO;
using Hl7.Fhir.Model;


namespace RestTest.Tests_Method
{
    [TestFixture]
    class PostBundleOrder
    {
        [Test]
        public void TestBundleMin()
        {
            //Bundle


            //var client = new RestClient();
            //client.BaseUrl = new Uri("http://192.168.8.93:2223/fhir/Patient?_format=json");
            //var request = new RestRequest(Method.POST);
            //request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            //var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
            //request.AddParameter("application/json; charset=utf-8", s, ParameterType.RequestBody);
            //var r = client.Execute(request);
        }
    }
}
