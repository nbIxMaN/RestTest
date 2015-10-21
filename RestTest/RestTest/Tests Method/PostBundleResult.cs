using Hl7.Fhir.Model;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest.Tests_Method
{
    class PostBundleResult
    {
        [Test]
        public void BundleResultMin()
        {
            string patient = "02255d1f-548c-4b04-9ac2-7c97d3efad1a";
            var s = new SetData();
            Bundle bundleResult = s.SetBundleResult(patient);
            string ex = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bundleResult);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", ex);
        }
    }
}
