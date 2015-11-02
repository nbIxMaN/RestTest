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
        /// Order, DiagnosticOrder(минус ссылка на Specimen)
        /// </summary>
        [Test]
        public void BundleOrder_Min()
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
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            if (bundleAnsw.Contains("error"))
                Assert.Fail(bundleAnsw);
        }

        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen),  Patient
        /// </summary>
        [Test]
        public void BundleOrder_Patient()
        {
            //задаём пациента
            Patient p = (new SetData()).SetPatient();
            p.Id = "d89de286-01ef-4737-a4f9-a10474c5fbc5";

            //задаём ссылки
            string patient = p.Id;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter, null, null);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, p);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            if (bundleAnsw.Contains("error"))
                Assert.Fail(bundleAnsw);
        }

        /// <summary>
        /// Order, DiagnosticOrder, Specimen
        /// </summary>
        [Test]
        public void BundleOrder_Specimen()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                 Ids.specimen, null);
            Specimen specimen = (new SetData()).SetSpecimen_Full(patient);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, null, null, null, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter), Condition, Specimen, Encounter
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounterCondition()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, Ids.encounter,
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
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, Ids.encounter,
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
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, Ids.encounter,
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
                                                                Ids.specimen, null, Ids.coverage);
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
                                                                Ids.specimen, new string[] { Ids.observation }, Ids.coverage);
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
            if (bundleAnsw.Contains("error"))
            {
                Assert.Fail(bundleAnsw);
            }
        }

        /// <summary>
        /// Order, DiagnosticOrder, Condition, Specimen, Encounter, Coverage, Practitioner, Observation
        /// Patient
        /// </summary>
        [Test]
        public void BundleOrder_MaxPatient()
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
                                                                Ids.specimen, new string[] { Ids.observation }, Ids.coverage );
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
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        //--------------------------------Put
        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen)
        /// </summary>
        [Test]
        public void BundleOrder_PutPractitioner()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = "ab1af9a5-91b0-4c7f-aba7-6eb4b8f43aab";

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, References.encounter,
                                                                null, null);
            Practitioner practitioner = (new SetData()).SetPractitioner();
            practitioner.Id = pract;
            practitioner.Name.Family = new List<string> { "New FamilyName" };
            practitioner.Meta = new Meta
            {
                //постоянно меняется, пока что это поле заполняется, выяснением этого значения вручную
                VersionId = pract
            };

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, null, null, null, null, null);
            Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
            {
                Resource = practitioner,
                Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.PUT, Url = References.practitioner }
            };
            b.Entry.Add(component);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }

        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Condition
        /// </summary>
        [Test]
        public void BundleOrder_PutEncounter()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;
            string enc = "1b377871-c721-40c6-8189-7d96369180b0";

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, enc,
                                                                null, null);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            encounter.Id = enc;
            encounter.Meta = new Meta
            {
                //постоянно меняется, пока что это поле заполняется, выяснением этого значения вручную
                VersionId = "0badfa59-ede5-44fd-b24b-390b7fe210ea"
            };
           encounter.Reason[0].Coding[0] = new Coding{ System = Dictionary.REASON, Code = "2", Version = "1" };
          //  encounter.Status = Encounter.EncounterState.Finished;

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, condition, null, null, null, null);
            Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
            {
                Resource = encounter,
                Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.PUT, Url = References.encounter }
            };
            b.Entry.Add(component);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            Assert.IsFalse(bundleAnsw.Contains("error"));
        }
    }
}
