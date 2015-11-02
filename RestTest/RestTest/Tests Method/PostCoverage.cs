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
        public void PostCoverage_Test()
        {
            Coverage cov = (new SetData()).SetCoverage(References.patient);
            cov.Id = null;
            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(cov);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/Coverage?_format=json", s);
            string answ = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }
    }
}
