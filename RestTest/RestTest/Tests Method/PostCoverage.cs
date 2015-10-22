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
            Coverage cov = (new SetData()).SetCoverage(References.patient);

            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(cov);
            string url = "http://192.168.8.93:2223/fhir/Coverage?_format=json";
            (new Program()).RequestExec(Method.POST, url, s);
        }
    }
}
