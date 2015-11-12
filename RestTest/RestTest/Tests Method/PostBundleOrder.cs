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
           
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }

        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Patient
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
         
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
           
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
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

            //Гетаем врача, чтобы узнать VersionId
            var client = new RestClient() { BaseUrl = new Uri("http://192.168.8.93:2223/fhir/" + References.practitioner) };
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            IRestResponse respPractVersion = client.Execute(request);

            // тут происходит магия (проблема с кодировкой)
            var encod = new UTF8Encoding(false).GetString(respPractVersion.RawBytes, 3, respPractVersion.RawBytes.Length - 3);
            Practitioner practAnsw = (Practitioner)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromXml(encod);

            //собственно тут достаём этот VersionId
            string versionId = practAnsw.Meta.VersionId;

            //а тут задаём обновлённого врача
            Practitioner practitioner = (new SetData()).SetPractitioner();
            practitioner.Id = pract;
            practitioner.Name.Family = new List<string> { "New FamilyName" + DateTime.Now };
            practitioner.Meta = new Meta { VersionId = versionId };

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
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }

        /// <summary>
        /// Order, DiagnosticOrder(минус ссылка на Specimen), Condition
        /// Чтобы тест заработал нужно создать новый encounter
        /// </summary>
        [Test]
        public void BundleOrder_PutEncounter()
        {
            //задаём ссылки
            string patient = References.patient;
            string pract = References.practitioner;
            string enc = "b111e27a-7bb2-478d-8bc1-db08ad009c19";

            //задаём ресурсы
            Order order = (new SetData()).SetOrder(patient, pract, References.organization);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder_Min(patient, pract, enc,
                                                                null, null);
            Condition condition = (new SetData()).SetCondition_MinDiag(patient);

            //Гетаем encounter, чтобы узнать VersionId
            var client = new RestClient() { BaseUrl = new Uri("http://192.168.8.93:2223/fhir/" + References.encounter) };
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            IRestResponse respEncountVersion = client.Execute(request);

            // тут происходит магия (проблема с кодировкой)
            var encod = new UTF8Encoding(false).GetString(respEncountVersion.RawBytes, 3, respEncountVersion.RawBytes.Length - 3);
            Encounter encountAnsw = (Encounter)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromXml(encod);

            //собственно тут достаём этот VersionId
            string versionId = encountAnsw.Meta.VersionId;

            //а тут задаём обновлённый encounter
            Encounter encounterUpdate = (new SetData()).SetEncounter(patient, new string[] { Ids.condition_min }, References.organization);
            encounterUpdate.Id = enc;
            encounterUpdate.Meta = new Meta { VersionId = versionId };
            encounterUpdate.Reason[0].Coding[0] = new Coding { System = Dictionary.REASON, Code = "2", Version = "1" };
            encounterUpdate.Status = Encounter.EncounterState.Onleave;

            //задаём Bundle 
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, null, condition, null, null, null, null);
            Bundle.BundleEntryComponent component = new Bundle.BundleEntryComponent
            {
                Resource = encounterUpdate,
                Transaction = new Bundle.BundleEntryTransactionComponent() { Method = Bundle.HTTPVerb.PUT, Url = References.encounter }
            };
            b.Entry.Add(component);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            IRestResponse resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
            string bundleAnsw = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Assert.Fail(resp.Content);
            Assert.Pass(resp.Content);
        }
    }
}
