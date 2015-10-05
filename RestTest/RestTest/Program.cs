using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;

namespace RestTest
{
        /// <summary>
        /// Default JSON serializer for request bodies
        /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
        /// </summary>
    public class RestSharpJsonNetSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        /// <summary>
        /// Default serializer
        /// </summary>
        public RestSharpJsonNetSerializer()
        {
            ContentType = "application/json";
            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }

        /// <summary>
        /// Default serializer with overload for allowing custom Json.NET settings
        /// </summary>
        public RestSharpJsonNetSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = "application/json";
            _serializer = serializer;
        }

        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    _serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }
    }


    //class URLandCODEABLE : Extension
    //{
    //    public string url;
    //    public Code valueCodeableConcept;
    //}

    //class URLandVALUE : Extension
    //{
    //    public string url;
    //    public Link valueReference;
    //}
    //class Item
    //{
    //    public Extension[] extension;
    //    public Coding[] coding;
    //}

    class Program
    {
        static void Main(string[] args)
        {
            //Patient p = new Patient()
            //{
            //    address = new Address[]
            //    {
            //        new Address()
            //        {
            //            Use = "home",
            //            text = "МОЖАЙСКАЯ 2-16"
            //        }
            //    },
            //    identifier = new Identifier[]
            //    {
            //        new Identifier()
            //        {
            //            system = "urn:oid:1.2.643.2.69.1.2.6",
            //            value = "IdPatientMis11111",
            //            assigner = new Reference()
            //            {
            //                reference = "Link/4bcbf113-f99c-41fa-a92d-43f5684fffc5"
            //            }
            //        }
            //    },
            //    birthDate = new DateTime(1980, 08, 31),
            //    gender = "male",
            //    name = new HumanName()
            //    {
            //        family = new string[]
            //        {
            //            "Пушкин"
            //        },
            //        given = new string[]
            //        {
            //            "Александр",
            //            "Сергеевич"
            //        }
            //    }
            //};
            Patient pa = (new SetData()).SetPatient();
            pa.address = null;
            pa.name = null;
            pa.resourceType = null;
            var c = new RestClient();
            c.BaseUrl = new Uri("http://fhir.zdrav.netrika.ru/fhir/Patient");
            var request = new RestRequest(Method.POST);
            request.JsonSerializer = new RestSharpJsonNetSerializer();
            var s = request.JsonSerializer.Serialize(pa);
            request.AddHeader("Authorization", "N3 f0a258e5-92e4-47d3-9b6c-89362357b2b3");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(pa);
            //request.AddParameter("application/json", s, RestSharp.ParameterType.RequestBody);
            var r = c.Execute(request);
        }
    }
}
