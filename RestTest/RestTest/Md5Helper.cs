using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace N3.EMK.Infrastructure.Helpers {

	public class Md5Helper {
		#region md5

		public static byte[] GetMd5Hash(string content)
		{
			var bytes = Encoding.UTF8.GetBytes(content);
			using (var md5Provider = new MD5CryptoServiceProvider()) {
				return md5Provider.ComputeHash(bytes);
			}
		}

		public static byte[] GetMd5Hash(byte[] content)
		{
			using (var md5Provider = new MD5CryptoServiceProvider()) {
				return md5Provider.ComputeHash(content);
			}
		}

		public static bool VerifyMd5Hash(string content, byte[] hash)
		{
			var contentHash = GetMd5Hash(content);
			return contentHash.SequenceEqual(hash);
		}

		#endregion

		#region GOST3411

		public static byte[] GetGost3411Hash(string content)
		{
			using (var hashAlgorithm = HashAlgorithm.Create("GOST3411")) {
				if (hashAlgorithm == null) {
					throw new CryptographicException("Криптопровайдер GOST3411 не установлен, или срок лицензии Крипто-Про истек");
				}
				var bytes = Encoding.UTF8.GetBytes(content);
				return hashAlgorithm.ComputeHash(bytes);
			}
		}

		public static byte[] GetGost3411Hash(byte[] content)
		{
			using (var hashAlgorithm = HashAlgorithm.Create("GOST3411")) {
				if (hashAlgorithm == null) {
					throw new CryptographicException("Криптопровайдер GOST3411 не установлен, или срок лицензии Крипто-Про истек");
				}
				return hashAlgorithm.ComputeHash(content);
			}
		}


		public static bool VerifyGost3411Hash(string content, byte[] hash) {
			var contentHash = GetGost3411Hash(content);
			return contentHash.SequenceEqual(hash);
		}

		public static bool VerifyGost3411Hash(byte[] content, byte[] hash) {
			var contentHash = GetGost3411Hash(content);
			return contentHash.SequenceEqual(hash);
		}

		#endregion

	}
}