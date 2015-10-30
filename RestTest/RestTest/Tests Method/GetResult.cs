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
    class GetResult
    {
        [Test]
        public void MaxGetOrderWithOrderMisId()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter,
                                                                 null, null);
            //задаём Bundle
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            string source = order.Identifier[0].Assigner.Reference;
            source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;

            Parameters a = new Parameters();

            a.Add("SourceCode", new FhirString("4a94e705-ee3e-46fc-bba0-0298e0fd5bd2"));
            a.Add("TargetCode", new FhirString("25bf5360-69ec-4bea-b4c4-c5398c014d60"));
            a.Add("OrderMisID", new FhirString("599760eb-62b2-11e5-9401-00155d128302"));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getresult", s2);
            NUnit.Framework.Assert.Fail(resp2.Content);
        }
    }
}
