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
            string pract = References.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter, 
                                                                 null, new string[] { Ids.condition_min });
            Condition condition = (new SetData()).SetCondition_Min(patient);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, condition, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }

        /// <summary>
        /// Order, DiagnosticOrder, Condition, Specimen
        /// </summary>
        [Test]
        public void BundleOrder_Specimen()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, References.encounter,
                                                                 Ids.specimen, new string[] { Ids.condition_min });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, null, condition, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter), Condition, Specimen, Encounter
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounter()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                 Ids.specimen, new string[] { Ids.condition_min });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, null, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter, observation), Condition, Specimen, Encounter, Observation
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounterObservation()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.condition_min, Ids.observation });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, observation, null, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
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
            string pract = Ids.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.condition_min});
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Practitioner practitioner = (new SetData()).SetPractitioner();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, null, practitioner, null);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }

        /// <summary>
        /// Order, DiagnosticOrder (ids encounter), Condition, Specimen, Encounter, Coverage
        /// </summary>
        [Test]
        public void BundleOrder_SpecimenEncounterCoverage()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.condition_min });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, null, null, coverage);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }

        /// <summary>
        /// Order, DiagnosticOrder, Condition, Specimen, Encounter, Coverage, Practitioner, Observation
        /// </summary>
        [Test]
        public void BundleOrder_Max()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = Ids.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.condition_min, Ids.observation});
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            Practitioner practitioner = (new SetData()).SetPractitioner();
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, observation, practitioner, coverage);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
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

            var s1 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
            string url = "http://192.168.8.93:2223/fhir/Patient?_format=json";
            IRestResponse resp = (new Program()).RequestExec(Method.POST, url, s1);

            dynamic patient = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content);

            //задаём ссылки
            patient = patient.id;
            string pract = Ids.partitioner;

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            //supportInfo уточнить (condition же обязательный) пока что тут передаю его на всякий
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, Ids.encounter,
                                                                Ids.specimen, new string[] { Ids.condition_min, Ids.observation });
            Specimen specimen = (new SetData()).SetSpecimen_Min(patient);
            Condition condition = (new SetData()).SetCondition_Min(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            Coverage coverage = (new SetData()).SetCoverage(patient);
            Practitioner practitioner = (new SetData()).SetPractitioner();
            Observation observation = (new SetData()).SetObservation_BundleOrder();

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter, condition, observation, practitioner, coverage);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }

    }
}
