using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Serializers;
using System.IO;
using Hl7.Fhir.Model;
using RestSharp;

namespace RestTest
{
    class Program
    {
        public IRestResponse RequestExec(RestSharp.Method method, string url, string s)
        {
            var client = new RestClient() { BaseUrl = new Uri(url) };
            var request = new RestRequest(method) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            request.AddParameter("application/json; charset=utf-8", s, ParameterType.RequestBody);
            return client.Execute(request);
        }

        static void Main(string[] args)
        {
        }
    }
}
