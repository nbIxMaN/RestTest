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
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
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
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }
    }
}
