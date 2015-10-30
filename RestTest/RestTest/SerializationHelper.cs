using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace N3.EMK.Infrastructure.Helpers
{
	public class SerializationHelper<T>
		where T : class
	{
		#region Fields
		private readonly XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings {Indent = true, OmitXmlDeclaration = false, Encoding = new UTF8Encoding()};

		#endregion

		#region Private Static Methods

		private static string RemoveText(Match m)
		{
			return "";
		}

		#endregion

		#region Public Methods

		public byte[] ToBase64Binary(T obj)
		{
			var xmlString = string.Empty;
			var xmlDocument = new XmlDocument();

			var xmlSerializer = new XmlSerializer(typeof (T));
		 
				using (var utf8StringWriter = new Utf8StringWriter()) {
					using (var xmlWriter = XmlWriter.Create(utf8StringWriter, _xmlWriterSettings)) {
						xmlSerializer.Serialize(xmlWriter, obj);
					}
					xmlString = utf8StringWriter.ToString();
				}
		 

			xmlDocument.LoadXml(xmlString);
			var element = xmlDocument.DocumentElement;

			var xmlStyleSheetProcInstruction = xmlDocument.CreateProcessingInstruction("xml-stylesheet",
				"type=\"text/xsl\" href=\"http://tech-iemc-test.rosminzdrav.ru/schemasrepos/xslts/test/XSL/1.1/main.xsl\"");
			xmlDocument.InsertBefore(xmlStyleSheetProcInstruction, element);

			// ReSharper disable once PossibleNullReferenceException
			element.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
			element.SetAttribute("xmlns:ext", "urn:hl7-RU-EHR:v1");

			var extAttribute = xmlDocument.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
			extAttribute.Value =
				"urn:hl7-org:v3 http://tech-iemc-test.rosminzdrav.ru/schemasrepos/schemas/cda/1.1/CDA_RU01.xsd urn:hl7-RU-EHR:v1 http://tech-iemc-test.rosminzdrav.ru/schemasrepos/schemas/cda/1.1/POCD_MT000040_RU01_Extension.xsd";
			element.Attributes.Append(extAttribute);


			using (var utf8StringWriter = new Utf8StringWriter()) {
				xmlDocument.Save(utf8StringWriter);
				xmlString = utf8StringWriter.ToString();
			}

			var base64Binary = Encoding.UTF8.GetBytes(xmlString);
			return base64Binary;
		}

		public XmlElement ClassToXmlElement(T obj)
		{
			var xmlDocument = new XmlDocument();
			var serializer = new XmlSerializer(typeof (T));
			using (var stream = new MemoryStream()) {
				serializer.Serialize(stream, obj);
				stream.Flush();
				stream.Seek(0, SeekOrigin.Begin);
				xmlDocument.Load(stream);
			}
			var element = xmlDocument.DocumentElement;
			// ReSharper disable once PossibleNullReferenceException
			element.Prefix = "ext";
			element.RemoveAttribute("xmlns:xsi");
			element.RemoveAttribute("xmlns:xsd");
			element.RemoveAttribute("xmlns");

			return element;
		}

		public string Serialize(T toSerialize)
		{
			var xmlSerializer = new XmlSerializer(toSerialize.GetType());
			var settings = new XmlWriterSettings
						   {
							   Indent = true,
							   OmitXmlDeclaration = false
						   };

			using (var stream = new Utf8StringWriter())
			using (var writer = XmlWriter.Create(stream, settings)) {
				xmlSerializer.Serialize(writer, toSerialize, new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty}));
				return stream.ToString();
			}
		}

		public T Deserialize(string data)
		{
			var xmlSerializer = new XmlSerializer(typeof (T));

			T result = null;
			using (TextReader reader = new StringReader(data)) {
				try {
					result = (T) xmlSerializer.Deserialize(reader);
					return result;
				}
				catch {
					// ignored
				}
			}

			//Если не получилось десерелизовать изза пустых элементов
			//Удаляем пустые тэги вида <tag></tag>
			//_logger.Trace("Не удалось десерелизовать сообщение, пробуем почистить пустые теги - '{0}'", data);
			var cleanXml = Regex.Replace(data, @"<([a-zA-Z_]+)\b[^>]*></\1>", RemoveText);

			//_logger.Info("Сообщение после удаление тегов  <tag></tag> '{0}'",cleanXml);
			//Удаляем пустые тэги вида <tag/>
			cleanXml = Regex.Replace(cleanXml, @"<[a-zA-Z_].[^(><.)]+/>", RemoveText);
			//_logger.Trace("Сообщение после удаление пустых тегов '{0}'", cleanXml);

			using (TextReader reader = new StringReader(cleanXml)) {
				 
					result = (T) xmlSerializer.Deserialize(reader);
				 
			 
			}
			return result;
		}


		public void Test_XSD_Validate() {
			var base64String = File.ReadAllText("out_gost_base64.txt");
			var signed = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
			var xsdScheme = File.ReadAllText("SignData.xsd");
			var document = XDocument.Load(new StringReader(signed));

			var schemas = new XmlSchemaSet();
			schemas.Add("", XmlReader.Create(new StringReader(xsdScheme)));
			var errors = false;
			document.Validate(schemas, (o, e) => {
				Console.WriteLine("{0}", e.Message);
				errors = true;
			});

			//var result = SignatureHelper.VerifyN3Gost(signed, true);


		}


		#endregion

		#region Nested type: Utf8StringWriter

		private class Utf8StringWriter : StringWriter
		{
			#region Properties

			public override Encoding Encoding
			{
				get { return Encoding.UTF8; }
			}

			#endregion
		}

		#endregion
	}
}