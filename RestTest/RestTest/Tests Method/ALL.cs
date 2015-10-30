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
    class ALL
    {
        /// <summary>
        /// Patient
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Condition
        /// OrderResponse, DiagnosticReport, Observation
        /// </summary>
        [Test]
        public void Bundle_Max()
        {
            Hl7.Fhir.Model.Parameters o = new Parameters();
            //задаём пациента
            Patient p = (new SetData()).SetPatient();
            p.Id = "d89de286-01ef-4737-a4f9-a10474c5fbc5";

            //задаём ссылки
            string patient = p.Id;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                 null, null);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);

            //передаём Bundle Order
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, condition, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            dynamic bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content);
            Assert.IsFalse(bundleAnsw.Contains("error"));

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(patient, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, "DiagnosticOrder/" + diagnosticOrder.Id);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //передаём Bundle Result 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);

            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s2);
            string bundleAnsw2 = Newtonsoft.Json.JsonConvert.DeserializeObject(resp2.Content).ToString();
            Assert.IsFalse(bundleAnsw2.Contains("error"));
        }
    }
}
