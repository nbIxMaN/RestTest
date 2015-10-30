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
    class PostPatient
    {
        [Test]
        public void PostPatient_Test()
        {
            Patient p = (new SetData()).SetPatient();
            
            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
            string url = "http://192.168.8.93:2223/fhir/Patient?_format=json";
            IRestResponse resp = (new Program()).RequestExec(Method.POST, url, s);
            string answ = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            if (answ.Contains("error"))
            {
                Assert.Fail(answ);
            }
        }
    }
}
