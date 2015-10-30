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
    class PostBundleResult
    {
        /// <summary>
        /// OrderResponse, DiagnosticReport, Observation
        /// </summary>
        [Test]
        public void BundleResult_Min()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(patient, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, References.diagnosticOrder);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);         
            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            //var bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content);
            Assert.Fail(resp.Content);
        }

        [Test]
        public void BundleResult_MinRejected()
        {
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
            Assert.Fail(resp.Content);
        }

        /// <summary>
        /// OrderResponse, DiagnosticReport, Observation, Practitioner
        /// </summary>
        [Test]
        public void BundleResult_Max()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = Ids.practitioner;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(patient, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, References.diagnosticOrder);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);
            Practitioner practitioner = (new SetData()).SetPractitioner();

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, practitioner);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            //string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.Fail(resp.Content);
        }
    }
}
