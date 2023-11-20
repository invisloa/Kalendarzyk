using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Services
{
	/// <summary>
	/// Encryption and decryption service using AES algorithm
	/// Used to save events to a file
	/// </summary>
	public class AdvancedEncryptionStandardService
	{
		private readonly string _key;
		private readonly string _iv;

		public AdvancedEncryptionStandardService(string key, string iv)
		{
			_key = key;
			_iv = iv;
		}

		public string EncryptString(string plainText)
		{
			using var aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(_key);
			aesAlg.IV = Encoding.UTF8.GetBytes(_iv);

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using var msEncrypt = new MemoryStream();
			using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
			using (var swEncrypt = new StreamWriter(csEncrypt))
			{
				swEncrypt.Write(plainText);
			}

			return Convert.ToBase64String(msEncrypt.ToArray());
		}

		public string DecryptString(string cipherText)
		{
			using var aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(_key);
			aesAlg.IV = Encoding.UTF8.GetBytes(_iv);

			var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
			using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			using var srDecrypt = new StreamReader(csDecrypt);

			return srDecrypt.ReadToEnd();
		}
	}
}