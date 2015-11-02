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
        public void MaxGetOrderWithOrderMisId()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                 null, null);
            //задаём Bundle
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            string source = order.Identifier[0].Assigner.Reference;
            source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/')+1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;

            Parameters a = new Parameters();

            a.Add("SourceCode", new FhirString(source));
            a.Add("TargetCode", new FhirString(target));
            a.Add("OrderMisID", new FhirString(orderMis));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getorder", s2);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
        [Test]
        public void MinGetOrderWithOrderMisId()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                 null, null);
            //задаём Bundle
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;

            Parameters a = new Parameters();

            a.Add("TargetCode", new FhirString(target));
            a.Add("OrderMisID", new FhirString(orderMis));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getorder", s2);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
        [Test]
        public void MinGetOrderWithBarCode()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                 null, null);
            //задаём Bundle
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;

            Parameters a = new Parameters();

            a.Add("TargetCode", new FhirString(target));
            a.Add("OrderMisID", new FhirString(orderMis));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getorder", s2);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
    }
}
