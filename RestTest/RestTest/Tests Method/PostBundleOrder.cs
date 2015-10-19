using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [Test]
        public void BundleOrderMin()
        {
            string patient = "02255d1f-548c-4b04-9ac2-7c97d3efad1a";
            string pract = "Practitioner/131d7d5d-0f21-451d-86ec-27fa3e069e1a";
            string specimenRef = "f8cd600f-f5b5-4b18-9662-18212c1935f9";
            string supportInfo = "56350c6f-7333-4002-a622-96968b85381e"; // Observation/ Condition

            string indicat = "71cf33b8-2eae-432d-88d5-747ef8147d0b";//Condition

            Order order = (new SetData()).SetOrder(patient, pract);
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient, pract, specimenRef, supportInfo);
            Encounter encounter = (new SetData()).SetEncounter(patient, new string[] { indicat });
            Condition condition = (new SetData()).SetCondition(patient);

            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, null, encounter, condition, null, null, null);


            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }


        //не проверена связность ссылок!
        [Test]
        public void BundleOrderMin()
        {
            string patient = "02255d1f-548c-4b04-9ac2-7c97d3efad1a";

            Order order = (new SetData()).SetOrder(patient, "Practitioner/131d7d5d-0f21-451d-86ec-27fa3e069e1a");
            DiagnosticOrder diagnosticOrder = (new SetData()).SetDiagnosticOrder(patient);
            Encounter encounter = (new SetData()).SetEncounter(patient);
            Condition condition = (new SetData()).SetCondition(patient);
            Specimen specimen = (new SetData()).SetSpecimen(patient);
            Observation observation = (new SetData()).SetObservation_BundleOrder();
            Practitioner practitioner = (new SetData()).SetPractitioner();
            Coverage coverage = (new SetData()).SetCoverage(patient);

            Bundle b = (new SetData()).SetBundleOrder(order, diagnosticOrder, specimen, encounter,
                condition, observation, practitioner, coverage);

            string s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(b);
            (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);
        }
    }
}
