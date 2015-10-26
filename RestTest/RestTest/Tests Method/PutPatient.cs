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
    class PutPatient
    {
        [Test]
        public void PutPatient_Address()
        {
            Patient p = (new SetData()).SetPatient();
            Address address = p.Address[0];
            p.Address = null;

            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
            string url = "http://192.168.8.93:2223/fhir/Patient?_format=json";
            IRestResponse r = (new Program()).RequestExec(Method.POST, url, s);
            dynamic answPat = Newtonsoft.Json.JsonConvert.DeserializeObject(r.Content);

            string i = answPat.id;

            //обновление
            p.Id = i;
            p.Meta = new Meta
            {
                VersionId = answPat.meta.versionId
            };
            p.Address = new List<Address> { address };
            
            var s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
            url = "http://192.168.8.93:2223/fhir/Patient/" + i + "?_format=json";
            (new Program()).RequestExec(Method.PUT, url, s2);
        }
    }
}
