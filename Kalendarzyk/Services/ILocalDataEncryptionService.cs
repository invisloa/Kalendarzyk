namespace Kalendarzyk.Services
{
	public interface ILocalDataEncryptionService
	{
		string DecryptString(string cipherText);
		string EncryptString(string plainText);
	}
}