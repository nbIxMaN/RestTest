using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Hl7.Fhir.Model;
using RestSharp;

namespace RestTest.Tests_Method
{
    [TestFixture]
    class GetOrder
    {
        [Test]
        public void Test()
        {
            Parameters a = new Parameters();
            a.Add("TargetCode", new FhirString("1.2.643.2.69.1.2.2"));
            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/getorder?_format=json", s);
            NUnit.Framework.Assert.Fail(resp2.Content);
        }
    }
}
