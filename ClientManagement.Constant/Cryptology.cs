namespace ClientManagement.Constant;

public static class Cryptology
{
	public static EncryptionData Encrypt(string stringToEncrypt, string Key)
	{
		EncryptionData encryptionData = new EncryptionData();
		CryptLib cryptLib = new CryptLib();
		string text = CryptLib.GenerateRandomIV(16);
		Key = CryptLib.getHashSha256(Key, 16);
		encryptionData.Text = cryptLib.encrypt(stringToEncrypt, Key, text);
		encryptionData.Iv = text;
		return encryptionData;
	}

	public static string Decrypt(string stringToDecrypt, string Iv, string key)
	{
		CryptLib cryptLib = new CryptLib();
		key = CryptLib.getHashSha256(key, 16);
		stringToDecrypt = cryptLib.decrypt(stringToDecrypt, key, Iv);
		return stringToDecrypt;
	}

	public static string EncryptID(int input)
	{
		return CryptLib.TripleDESEncryptID(input);
	}

	public static int DecryptID(string input)
	{
		return CryptLib.TripleDESDecryptID(input);
	}
}
