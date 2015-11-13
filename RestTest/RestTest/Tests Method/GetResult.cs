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
    class GetResult
    {
        [Test]
        public void _GetResult()
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
            Bundle requestResult = (Bundle)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp.Content);

            //задаём ссылки
            string id = requestResult.Entry[0].Resource.Id;            
            string orderId = "Order/" + id;
            string diagnosticOrderId = "DiagnosticOrder/" + requestResult.Entry[1].Resource.Id;

            //задаём ресурсы
            OrderResponse orderResp = (new SetData()).SetOrderResponseInProgress(orderId, References.organization);
            DiagnosticReport diagRep = (new SetData()).SetDiagnosticReport(patient, pract, diagnosticOrderId);
            Observation observ = (new SetData()).SetObservation_BundleResult_Reason(pract);

            //задаём Bundle 
            Bundle bRes = (new SetData()).SetBundleResult(orderResp, diagRep, observ, null);
            s = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(bRes);
            resp = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir?_format=json", s);

            Newtonsoft.Json.JsonConvert.DeserializeObject(resp.Content).ToString();
            string source = order.Identifier[0].Assigner.Reference;
            source = source.Substring(source.IndexOf('/') + 1, source.Length - source.IndexOf('/') - 1);
            string target = order.Target.Reference;
            target = target.Substring(target.IndexOf('/') + 1, target.Length - target.IndexOf('/') - 1);
            string orderMis = order.Identifier[0].Value;

            Parameters a = new Parameters();
            a.Add("SourceCode", new FhirString(source));
            a.Add("TargetCode", new FhirString(target));
            a.Add("OrderMisID", new FhirString(order.Identifier[0].Value));
            
            string s2 = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(a);
            IRestResponse resp2 = (new Program()).RequestExec(Method.POST, "http://192.168.8.93:2223/fhir/$getresult?_format=json", s2);

            //Проверка на то, что возвращается НЕ пустой ответ!
            Parameters respGet = (Parameters)Hl7.Fhir.Serialization.FhirParser.ParseResourceFromJson(resp2.Content);

            if (resp2.StatusCode != System.Net.HttpStatusCode.OK || respGet.Parameter.Count == 0)
                Assert.Fail(resp2.Content);
            Assert.Pass(resp2.Content);
        }
    }
}
