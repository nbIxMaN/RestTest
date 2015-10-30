using System;

namespace N3.EMK.Domain.BusinessEntities
{
	[Serializable]
	public class SignData {
		public string Data { get; set; }

		public string PublicKey { get; set; }

		public string Hash { get; set; }

		public string Sign { get; set; }

	}
}