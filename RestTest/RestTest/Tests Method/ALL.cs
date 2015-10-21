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
        [Test]
        public void PostPatient_Test()
        {
            Patient p = (new SetData()).SetPatient();

            var s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(p);
            string url = "http://192.168.8.93:2223/fhir/Patient?_format=json";
            IRestResponse resp = (new Program()).RequestExec(Method.POST, url, s);
            dynamic pat = Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content);
            Ids.patient = pat.id;

            string specimenRef = Ids.specimen;
            string supportInfo = Ids.condition; // Observation/ Condition

            string indicat = "71cf33b8-2eae-432d-88d5-747ef8147d0b";//Condition
            Practitioner partitioner = (new SetData()).SetPractitioner();
            Order order = (new SetData()).SetOrder(Ids.patient, Ids.partitioner);
            Condition condition = (new SetData()).SetCondition(Ids.patient);
            Encounter encounter = (new SetData()).SetEncounter(Ids.patient, new string[] { Ids.condition });
            Specimen speciment = (new SetData()).SetSpecimen(Ids.patient);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(Ids.patient, Ids.partitioner, specimenRef, Ids.condition);
            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, speciment, encounter, condition, null, partitioner, null);

            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }
    }
}
