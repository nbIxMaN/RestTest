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
    class GetStatus
    {
        [Test]
        public void GetStatusWithOrderId()
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
            //Bundle p = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);
            //string id = p.Entry[0].Resource.Id;
            //string source = orderResp.Identifier[0].Assigner.Reference;
            //source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            //string orderId = id;
            //string orderMis = orderResp.Identifier[0].Value;
            Parameters a = new Parameters();

            //a.Add("SourceCode", new FhirString(source));
            //a.Add("OrderMisID", new FhirString(orderMis));
            a.Add("OrderId", new FhirString("99e4270e-a713-4fab-8ecf-e19161ece69c"));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus", s2);
            NUnit.Framework.Assert.Fail(resp2.Content);
        }

        [Test]
        public void GetStatusWithSourceCode()
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
            Bundle p = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);
            string id = p.Entry[0].Resource.Id;
            string source = order.Identifier[0].Assigner.Reference;
            source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            string orderId = id;
            string orderMis = order.Identifier[0].Value;
            Parameters a = new Parameters();

            a.Add("SourceCode", new FhirString("4a94e705-ee3e-46fc-bba0-0298e0fd5bd2"));
            a.Add("OrderMisID", new FhirString("IdOrderMis2"));
            //a.Add("OrderId", new FhirString(orderId));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus", s2);
            NUnit.Framework.Assert.Fail(resp2.Content);
        }
        [Test]
        public void GetReceivedStatus()
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
            Bundle p = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);
            string id = p.Entry[0].Resource.Id;
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;
            Parameters a = new Parameters();
            a.Add("TargetCode", new FhirString(target));
            a.Add("OrderMisID", new FhirString(orderMis));
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getorder", s2);
            a = new Parameters();
            a.Add("OrderId", new FhirString(id));
            s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus", s2);
            NUnit.Framework.Assert.Fail(resp2.Content);
        }

    }
}
