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
            //задаём ссылки
            string orderId = "Order/" + id;

            //задаём ссылки
            //задаём ресурсы
            string diagnosticOrderId = "DiagnosticOrder/" + p.Entry[1].Resource.Id;
            OrderResponse orderResp = (new SetData()).SetOrderResponseInProgress(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            string source = order.Identifier[0].Assigner.Reference;
            source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);


            Parameters a = new Parameters();

            //a.Add("TargetCode", new FhirString("25bf5360-69ec-4bea-b4c4-c5398c014d60"));
            a.Add("SourceCode", new FhirString(source));
            //a.Add("StartDate", new FhirDateTime(2015, 07, 31, 0, 0, 0));
            //a.Add("EndDate", new FhirDateTime(2015, 08, 03, 0, 0, 0));
            a.Add("StartDate", new FhirString(order.Date));
            //a.Add("EndDate", new Date("03.08.2015 0:00:00"));
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getresults?_format=json", s);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
        [Test]
        public void TestMax()
        {
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
            //задаём ссылки
            string orderId = "Order/" + id;

            //задаём ссылки
            //задаём ресурсы
            string diagnosticOrderId = "DiagnosticOrder/" + p.Entry[1].Resource.Id;
            OrderResponse orderResp = (new SetData()).SetOrderResponseInProgress(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            string source = order.Identifier[0].Assigner.Reference;
            source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            Parameters a = new Parameters();
            a.Add("TargetCode", new FhirString(target));
            a.Add("SourceCode", new FhirString(source));
            //a.Add("StartDate", new FhirDateTime(2015, 07, 31, 0, 0, 0));
            //a.Add("EndDate", new FhirDateTime(2015, 08, 03, 0, 0, 0));
            a.Add("StartDate", new FhirString(order.Date));
            //a.Add("EndDate", new FhirString("03.08.2015 0:00:00"));
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getresults?_format=json", s);
            if (resp2.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
    }
}
