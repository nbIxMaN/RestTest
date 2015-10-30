namespace N3.EMK.Domain.Helpers
{
	public interface ISignatureHelper {
		string SignXmlDSig(string xml);

		bool VerifySmlDSig(string xml);

		string SignN3Rsa(string data);

		bool VerifyN3Rsa(string signedData);

		string SignN3Gost(string data);

		bool VerifyN3Gost(string signedData, bool chainValidate = false);
	}

}