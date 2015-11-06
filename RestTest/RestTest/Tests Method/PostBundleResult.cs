using Hl7.Fhir.Model;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestTest.Tests_Method
{
    class PostBundleResult
    {
        /// <summary>
        /// OrderResponse, DiagnosticReport, Observation
        /// </summary>
        [Test]
        public void BundleResult_Min()
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

            Bundle requestResult = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);

            //задаём ссылки 
            string id = requestResult.Entry[0].Resource.Id;
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + requestResult.Entry[1].Resource.Id;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }

        [Test]
        public void BundleResultInProgress_Min()
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

            Bundle requestResult = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);

            //задаём ссылки 
            string id = requestResult.Entry[0].Resource.Id;
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + requestResult.Entry[1].Resource.Id;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponseInProgress(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }

        [Test]
        public void BundleResult_MinRejected()
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
            pract = References.practitioner;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponseRejected(orderId, References.organization);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, null, null, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            //var bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content);
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }

        /// <summary>
        /// OrderResponse, DiagnosticReport, Observation, Practitioner
        /// </summary>
        [Test]
        public void BundleResult_Max()
        {
            //задаём ссылки
            string pract = References.practitioner;
            string patient = References.patient;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                 null, null);
            //задаём Bundle
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            Bundle requestResult = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);

            //задаём ссылки
            string id = requestResult.Entry[0].Resource.Id;
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + requestResult.Entry[1].Resource.Id;
            pract = Ids.practitioner;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);
            Practitioner practitioner = (new SetData()).SetPractitioner();

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, practitioner);

            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }

        //-----------------------------------Put

        /// <summary>
        /// OrderResponse, DiagnosticReport, Observation, Practitioner
        /// </summary>
        [Test]
        public void BundleResult_PutPractitioner()
        {
            //задаём ссылки
            string pract = "ab1af9a5-91b0-4c7f-aba7-6eb4b8f43aab";
            string patient = References.patient;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, "Practitioner/" + pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, "Practitioner/" + pract, References.encounter,
                                                                 null, null);
            //задаём Bundle
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            Bundle requestResult = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);

            //задаём ссылки
            string id = requestResult.Entry[0].Resource.Id;
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + requestResult.Entry[1].Resource.Id;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //Гетаем врача, чтобы узнать VersionId
            var client = new RestClient() { BaseUrl = new Uri("http://192.168.8.93:2223/fhir/Practitioner/" + pract) };
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            IRestResponse respPractVersion = client.Execute(request);

            // тут происходит магия (проблема с кодировкой)
            string str = "<Practitioner xmlns=\"http://hl7.org/fhir\">" + respPractVersion.Content.Substring(43, respPractVersion.Content.Length - 43);
            Practitioner practAnsw = (Practitioner)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromXml(str);

            //собственно тут достаём этот VersionId
            string versionId = practAnsw.Meta.VersionId;

            //а тут задаём обновлённого врача
            Practitioner practitioner = (new SetData()).SetPractitioner();
            practitioner.Id = pract;
            practitioner.Name.Family = new List<string> { "FamilyName" + DateTime.Now };
            practitioner.Meta = new Meta { VersionId = versionId };

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
            {
                Resource = practitioner,
                Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.PUT, Url = References.practitioner }
            };
            bRes.Entry.Add(component);

            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }
    }
}
