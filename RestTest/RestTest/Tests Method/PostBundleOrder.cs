using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers;
using System.IO;
using Hl7.Fhir.Model;


namespace RestTest.Tests_Method
{
    [TestFixture]
    class PostBundleOrder
    {
        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Condition
        /// </summary>
        [Test]
        public void BundleOrder_Min()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter, null, null);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, condition, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Condition, Patient
        /// </summary>
        [Test]
        public void BundleOrder_Patient()
        {
            //добавляем пациента
            Patient p = (new SetData()).SetPatient();
            p.Id = "d89de286-01ef-4737-a4f9-a10474c5fbc5";

            //задаём ссылки
            string patient = p.Id;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter, null, null);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null,p);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder, Condition, Specimen
        /// </summary>
        [Test]
        public void BundleOrder_Specimen()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter,
                                                                 Ids.specimen, null);
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, null, condition, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter), Condition, Specimen, Encounter
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounter()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                 Ids.specimen, null);
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter, observation), Condition, Specimen, Encounter, Observation
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounterObservation()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.observation });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, observation, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter), Condition, Specimen, Encounter, Practitioner
        /// ids practitioner
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounterPractitioner()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = Ids.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, null);
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Practitioner practitioner = (new SetData()).SetPractitioner();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, null, practitioner, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter), Condition, Specimen, Encounter, Coverage
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounterCoverage()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, null);
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, null, null, coverage, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder, Condition, Specimen, Encounter, Coverage, Practitioner, Observation
        /// </summary>
        [Test]
        public void BundleOrder_Max()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = Ids.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.observation});
            Specimen specimen = (new SetData()).SetSpecimen_Full(patient);
            Condition condition = (new SetData()).SetCondition_Full(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            Practitioner practitioner = (new SetData()).SetPractitioner();
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, observation, practitioner, coverage, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder, Condition, Specimen, Encounter, Coverage, Practitioner, Observation
        /// Patient
        /// </summary>
        [Test]
        public void BundleOrder_MaxPatient()
        {
            //добавляем пациента
            Patient p = (new SetData()).SetPatient();
            p.Id = "d89de286-01ef-4737-a4f9-a10474c5fbc5";

            //задаём ссылки
            string patient = p.Id;
            string pract = Ids.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.observation });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            Practitioner practitioner = (new SetData()).SetPractitioner();
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, encounter, condition, observation, practitioner, coverage, p);
            Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
            {
                Resource = p,
                Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.POST, Url = "Patient" }
            };
            b.Entry.Add(component);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }
    }
}
