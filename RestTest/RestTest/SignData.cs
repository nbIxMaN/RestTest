using System;

namespace N3.EMK.Domain.BusinessEntities
{
	[Serializable]
	public class SignData {
		public string data { get; set; }

		public string public_key { get; set; }

		public string hash { get; set; }

		public string sign { get; set; }

	}
}