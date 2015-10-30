using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using CryptoPro.Sharpei;
using Monads.NET;
using N3.EMK.Domain.BusinessEntities;
using N3.EMK.Domain.Helpers;
using Newtonsoft.Json;

namespace N3.EMK.Infrastructure.Helpers
{
	public class SignatureHelper : ISignatureHelper
	{
		#region Public Methods

		public string SignXmlDSig(string xml)
		{
			var cspParams = new CspParameters {KeyContainerName = "XML_DSIG_RSA_KEY"};
			var key = new RSACryptoServiceProvider(cspParams);
			var blob = key.ExportCspBlob(true);
			var cert = new X509Certificate2(blob);
			var doc = new XmlDocument();

			doc.LoadXml(xml);

			var signedXml = new SignedXml(doc) {SigningKey = cert.PrivateKey};

			var reference = new Reference {Uri = ""};

			var env = new XmlDsigEnvelopedSignatureTransform();
			reference.AddTransform(env);

			signedXml.AddReference(reference);

			var keyInfo = new KeyInfo();
			keyInfo.AddClause(new RSAKeyValue(cert.PublicKey.Key as RSACryptoServiceProvider));
			signedXml.KeyInfo = keyInfo;

			signedXml.ComputeSignature();

			var xmlDigitalSignature = signedXml.GetXml();

			doc
				.With(x => x.DocumentElement)
				.Do(x => x.AppendChild(doc.ImportNode(xmlDigitalSignature, true)));

			if (doc.FirstChild is XmlDeclaration)
				doc.RemoveChild(doc.FirstChild);

			return doc.InnerXml;
		}

		public bool VerifySmlDSig(string xml)
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);

			var signedXml = new SignedXml(xmlDocument);

			var nodeList = xmlDocument.GetElementsByTagName("Signature");
			signedXml.LoadXml((XmlElement) nodeList[0]);

			return signedXml.CheckSignature();
		}

		public string SignN3Rsa(string data)
		{
			var cspParams = new CspParameters {KeyContainerName = "XML_DSIG_RSA_KEY"};
			var key = new RSACryptoServiceProvider(cspParams);
			var cspBlob = key.ExportCspBlob(false);
			var base64Blob = Convert.ToBase64String(cspBlob);

			var rsaFormatter = new RSAPKCS1SignatureFormatter(key);
			rsaFormatter.SetHashAlgorithm("MD5");

			var hash = Md5Helper.GetMd5Hash(data);
			var base64Hash = Convert.ToBase64String(hash);
			var sign = rsaFormatter.CreateSignature(hash);
			var base64Sign = Convert.ToBase64String(sign);

			var signData = new SignData
			               {
				               data = data,
				               public_key = base64Blob,
				               hash = base64Hash,
				               sign = base64Sign
			               };

			return new SerializationHelper<SignData>().Serialize(signData);
		}

		public bool VerifyN3Rsa(string signedData)
		{
			var signData = new SerializationHelper<SignData>().Deserialize(signedData);
			var cspBlob = Convert.FromBase64String(signData.public_key);
			var hash = Md5Helper.GetMd5Hash(signData.data);
			var sign = Convert.FromBase64String(signData.sign);

			var key = new RSACryptoServiceProvider();
			key.ImportCspBlob(cspBlob);

			var rsaDeformatter = new RSAPKCS1SignatureDeformatter(key);
			rsaDeformatter.SetHashAlgorithm("MD5");

			return rsaDeformatter.VerifySignature(hash, sign);
		}

		//INFO: метод для тестирования
		public string SignN3Gost(string data)
		{
			var storeCurrentUser = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			storeCurrentUser.Open(OpenFlags.ReadOnly);

			var coll = storeCurrentUser.Certificates
			                           .Find(X509FindType.FindByThumbprint, "4d 19 79 84 52 9a 80 4a c4 86 3a 82 6a 8d ab 85 3f 95 e5 01", false)[0];
			//b8 be f8 22 e8 63 2a 74 d4 2e 58 df 91 9c 2f e3 75 ea e1 e4 просрочен
			//4d 19 79 84 52 9a 80 4a c4 86 3a 82 6a 8d ab 85 3f 95 e5 01
			var gost = (Gost3410CryptoServiceProvider) coll.PrivateKey;

			var base64Blob = Convert.ToBase64String(coll.Export(X509ContentType.Cert));

			var gostSignatureFormatter = new GostSignatureFormatter(gost);
			gostSignatureFormatter.SetHashAlgorithm("Gost3411");

			var hash = Md5Helper.GetGost3411Hash(data);
			var base64Hash = Convert.ToBase64String(hash);
			var sign = gostSignatureFormatter.CreateSignature(hash);
			var base64Sign = Convert.ToBase64String(sign);

			var signData = new SignData
			               {
				               data = data,
				               public_key = base64Blob,
				               hash = base64Hash,
				               sign = base64Sign
			               };

			return JsonConvert.SerializeObject(signData);
		}

		/// <summary>
		/// </summary>
		/// <param name="signedData"></param>
		/// <param name="chainValidate">Проверять цепочку сертификатов на отозванные сертификаты</param>
		/// <returns></returns>
		public bool VerifyN3Gost(string signedData, bool chainValidate = false)
		{
			var x509 = new X509Certificate2();

			var signData = new SerializationHelper<SignData>().Deserialize(signedData);
			var cspBlob = Convert.FromBase64String(signData.public_key);
			x509.Import(cspBlob);
			if (chainValidate) {
				var chain = new X509Chain
				            {
					            ChainPolicy =
					            {
						            RevocationFlag = X509RevocationFlag.EntireChain,
						            RevocationMode = X509RevocationMode.Online,
						            VerificationFlags = X509VerificationFlags.NoFlag
					            }
				            };
				var verify = chain.Build(x509);
				if (!verify) return false;
			}

			var hash = Md5Helper.GetGost3411Hash(signData.data);
			var sign = Convert.FromBase64String(signData.sign);


			var rsaDeformatter = new GostSignatureDeformatter(x509.PublicKey.Key);
			rsaDeformatter.SetHashAlgorithm("Gost3411");

			return rsaDeformatter.VerifySignature(hash, sign);
		}

		#endregion
	}
}