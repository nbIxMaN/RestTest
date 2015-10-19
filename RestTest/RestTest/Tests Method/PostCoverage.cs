using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers;
using System.IO;
using Hl7.Fhir.Model;

namespace RestTest.Tests_Method
{
    [TestFixture]
    class PostCoverage
    {
        [Test]
        public void Test()
        {
            Coverage cov = (new SetData()).SetCoverage("b68a2114-2df7-4e72-827f-560e5d5efea1");

            var client = new RestClient();
            client.BaseUrl = new Uri("http://192.168.8.93:2223/fhir/Coverage?_format=json");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(cov);
            request.AddParameter("application/json; charset=utf-8", s, ParameterType.RequestBody);
            var r = client.Execute(request);
        }
    }
}
