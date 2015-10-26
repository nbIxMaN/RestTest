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
        //[Test]
        //public void PostPatient_Test()
        //{
        //    Patient p = (new SetData()).SetPatient();

        //    var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
        //    string url = "http://192.168.8.93:2223/fhir/Patient?_format=json";
        //    IRestResponse resp = (new Program()).RequestExec(Method.POST, url, s);
        //    dynamic pat = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content);
        //    Ids.patient = pat.id;

        //    string specimenRef = Ids.specimen;
        //    string supportInfo = Ids.condition; // Observation/ Condition

        //    string indicat = "71cf33b8-2eae-432d-88d5-747ef8147d0b";//Condition
        //    Practitioner partitioner = (new SetData()).SetPractitioner();
        //    Order order = (new SetData()).SetOrder(Ids.patient, Ids.partitioner);
        //    Condition condition = (new SetData()).SetCondition_Full(Ids.patient);
        //    Encounter encounter = (new SetData()).SetEncounter(Ids.patient, new string[] { Ids.condition });
        //    Specimen speciment = (new SetData()).SetSpecimen(Ids.patient);
        //    DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(Ids.patient, Ids.partitioner, specimenRef, Ids.condition);
        //    Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, speciment, encounter, condition, null, partitioner, null);

        //    s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
        //    (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        //}


        /// <summary>
        /// Patient
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Condition
        /// OrderResponse, DiagnosticReport, Observation
        /// </summary>
        [Test]
        public void Bundle_Max()
        {
            //задаём пациента
            Patient p = (new SetData()).SetPatient();
            p.Id = "d89de286-01ef-4737-a4f9-a10474c5fbc5";

            //задаём ссылки
            string patient = p.Id;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter,
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
