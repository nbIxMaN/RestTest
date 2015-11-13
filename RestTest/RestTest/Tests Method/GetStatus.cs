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
            
            Parameters a = new Parameters();
            a.Add("OrderId", new FhirString("99e4270e-a713-4fab-8ecf-e19161ece69c"));
            
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus?_format=json", s2);
            
            if (resp2.Content.Contains("Requested"))
                Assert.Pass(resp2.Content);
            else
                NUnit.Framework.Assert.Fail(resp2.Content);
        }

        [Test]
        public void GetInProgressStatus()
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
            
            //задаём ссылки
            string id = p.Entry[0].Resource.Id;
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + p.Entry[1].Resource.Id;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponseInProgress(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
           
            Parameters a = new Parameters();
            a.Add("OrderId", new FhirString(id));
            
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus?_format=json", s2);
            
            if (resp2.Content.Contains("Accepted"))
                Assert.Pass(resp2.Content);
            else
                NUnit.Framework.Assert.Fail(resp2.Content);
        }

        [Test]
        public void GetInjectedStatus()
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
            
            //задаём ссылки
            string id = p.Entry[0].Resource.Id;
            string orderId = "Order/" + id;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponseRejected(orderId, References.organization);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, null, null, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            
            Parameters a = new Parameters();
            a.Add("OrderId", new FhirString(id));
            
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus?_format=json", s2);
            if (resp2.Content.Contains("Rejected"))
                Assert.Pass(resp2.Content);
            else
                NUnit.Framework.Assert.Fail(resp2.Content);
        }

        [Test]
        public void MinGetStatusReceived()
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
            
            //задаём ссылки
            string id = p.Entry[0].Resource.Id;
            string orderId = "Order/" + id;

            Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;

            Parameters a = new Parameters();
            a.Add("TargetCode", new FhirString(target));
            a.Add("OrderMisID", new FhirString(orderMis));
  
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getorder?_format=json", s2);

            //Проверка на то, что возвращается НЕ пустой ответ!
            Parameters respGet = (Parameters)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp2.Content);

            if (resp2.StatusCode != System.Net.HttpStatusCode.OK || respGet.Parameter.Count == 0)
                Assert.Fail(resp2.Content + " Это упал GetOrder");
            
            var startIndex = resp2.Content.IndexOf("DiagnosticOrder");
            var endIndex = resp2.Content.IndexOf("\"", startIndex);
            var mys = resp2.Content.Substring(startIndex, endIndex - startIndex);
            var client = new RestClient() { BaseUrl = new Uri("http://192.168.8.93:2223/fhir/" + mys) };
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            IRestResponse respPractVersion = client.Execute(request);
            
            a = new Parameters();
            a.Add("OrderId", new FhirString(id));
            
            s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getstatus?_format=json", s2);
            
            if (resp2.Content.Contains("Received"))
                Assert.Pass(resp2.Content);
            else
                NUnit.Framework.Assert.Fail(resp2.Content);
        }
    }
}
