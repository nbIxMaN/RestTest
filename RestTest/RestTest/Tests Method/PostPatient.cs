using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;

namespace RestTest.Tests_Method
{
    [TestFixture]
    class PostPatient
    {
        [Test]
        public void Test()
        {
            Patient p = (new SetData()).SetPatient();
            var c = new RestClient();
            c.BaseUrl = new Uri("http://fhir.zdrav.netrika.ru/fhir/Patient");
            var request = new RestRequest(Method.POST);

            var s = request.JsonSerializer.Serialize(p);
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(p);
            var r = c.Execute(request);
        }
    }
}
