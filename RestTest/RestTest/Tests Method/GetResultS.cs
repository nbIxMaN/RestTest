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
    class GetResultS
    {
        [Test]
        public void TestMin()
        {
            Parameters a = new Parameters();
            //a.Add("TargetCode", new FhirString("25bf5360-69ec-4bea-b4c4-c5398c014d60"));
            a.Add("SourceCode", new FhirString("4a94e705-ee3e-46fc-bba0-0298e0fd5bd2"));
            //a.Add("StartDate", new FhirDateTime(2015, 07, 31, 0, 0, 0));
            //a.Add("EndDate", new FhirDateTime(2015, 08, 03, 0, 0, 0));
            a.Add("StartDate", new FhirString("31.07.2015 0:00:00"));
            //a.Add("EndDate", new Date("03.08.2015 0:00:00"));
            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getresults?_format=json", s);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
        [Test]
        public void TestMax()
        {
            Parameters a = new Parameters();
            a.Add("TargetCode", new FhirString("25bf5360-69ec-4bea-b4c4-c5398c014d60"));
            a.Add("SourceCode", new FhirString("4a94e705-ee3e-46fc-bba0-0298e0fd5bd2"));
            //a.Add("StartDate", new FhirDateTime(2015, 07, 31, 0, 0, 0));
            //a.Add("EndDate", new FhirDateTime(2015, 08, 03, 0, 0, 0));
            a.Add("StartDate", new FhirString("31.07.2015 0:00:00"));
            a.Add("EndDate", new FhirString("03.08.2015 0:00:00"));
            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getresults?_format=json", s);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
    }
}
