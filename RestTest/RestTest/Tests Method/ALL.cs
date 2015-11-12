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
    class All
    {
        /// <summary>
        /// Patient
        /// Order, DiagnosticOrder, Specimen, Condition, Encounter, Coverage, Practitioner, Observation
        /// OrderResponse, DiagnosticReport, Observation, Practitioner
        /// </summary>
        [Test]
        public void MaxWithMax()
        {
            //задаём пациента
            Patient p = (new SetData()).SetPatient();
            p.Id = "d89de286-01ef-4737-a4f9-a10474c5fbc5";

            //задаём ссылки
            string patient = p.Id;
            string pract = Ids.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.observation }, Ids.coverage);
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            Practitioner practitioner = (new SetData()).SetPractitioner();
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, observation, practitioner, coverage, p);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            Bundle requestResult = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);

            //задаём ссылки
            string id = requestResult.Entry[0].Resource.Id;
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + requestResult.Entry[1].Resource.Id;
            patient = "Patient/" + patient;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponse(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);
            Practitioner practitioner2 = (new SetData()).SetPractitioner();

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, practitioner2);

            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }
    }
}
