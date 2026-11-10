using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ClientManagement.Constant;

public class CryptLib
{
	private enum EncryptMode
	{
		ENCRYPT,
		DECRYPT
	}

	private UTF8Encoding _enc;

	private RijndaelManaged _rcipher;

	private byte[] _key;

	private byte[] _pwd;

	private byte[] _ivBytes;

	private byte[] _iv;

	private static readonly char[] CharacterMatrixForRandomIVStringGeneration = new char[64]
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
		'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
		'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
		'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
		'8', '9', '-', '_'
	};

	internal static string GenerateRandomIV(int length)
	{
		char[] array = new char[length];
		byte[] array2 = new byte[length];
		using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
		{
			rNGCryptoServiceProvider.GetBytes(array2);
		}
		for (int i = 0; i < array.Length; i++)
		{
			int num = array2[i] % CharacterMatrixForRandomIVStringGeneration.Length;
			array[i] = CharacterMatrixForRandomIVStringGeneration[num];
		}
		return new string(array);
	}

	public CryptLib()
	{
		_enc = new UTF8Encoding();
		_rcipher = new RijndaelManaged();
		_rcipher.Mode = CipherMode.CBC;
		_rcipher.Padding = PaddingMode.PKCS7;
		_rcipher.KeySize = 256;
		_rcipher.BlockSize = 128;
		_key = new byte[32];
		_iv = new byte[_rcipher.BlockSize / 8];
		_ivBytes = new byte[16];
	}

	private string encryptDecrypt(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
	{
		string result = "";
		_pwd = Encoding.UTF8.GetBytes(_encryptionKey);
		_ivBytes = Encoding.UTF8.GetBytes(_initVector);
		int num = _pwd.Length;
		if (num > _key.Length)
		{
			num = _key.Length;
		}
		int num2 = _ivBytes.Length;
		if (num2 > _iv.Length)
		{
			num2 = _iv.Length;
		}
		Array.Copy(_pwd, _key, num);
		Array.Copy(_ivBytes, _iv, num2);
		_rcipher.Key = _key;
		_rcipher.IV = _iv;
		if (_mode.Equals(EncryptMode.ENCRYPT))
		{
			byte[] bytes = _enc.GetBytes(_inputText);
			byte[] inArray = _rcipher.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
			result = Convert.ToBase64String(inArray);
		}
		if (_mode.Equals(EncryptMode.DECRYPT))
		{
			byte[] array = Convert.FromBase64String(_inputText);
			byte[] bytes2 = _rcipher.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
			result = _enc.GetString(bytes2);
		}
		_rcipher.Dispose();
		return result;
	}

	public string encrypt(string _plainText, string _key, string _initVector)
	{
		return encryptDecrypt(_plainText, _key, EncryptMode.ENCRYPT, _initVector);
	}

	public string decrypt(string _encryptedText, string _key, string _initVector)
	{
		return encryptDecrypt(_encryptedText, _key, EncryptMode.DECRYPT, _initVector);
	}

	public static string getHashSha256(string text, int length)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		SHA256Managed sHA256Managed = new SHA256Managed();
		byte[] array = sHA256Managed.ComputeHash(bytes);
		string text2 = string.Empty;
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text2 += $"{b:x2}";
		}
		if (length > text2.Length)
		{
			return text2;
		}
		return text2.Substring(0, length);
	}

	public static string sha256_hash(string value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (SHA256 sHA = SHA256.Create())
		{
			Encoding uTF = Encoding.UTF8;
			byte[] array = sHA.ComputeHash(uTF.GetBytes(value));
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
		}
		return stringBuilder.ToString();
	}

	private static string MD5Hash(string text)
	{
		MD5 mD = new MD5CryptoServiceProvider();
		mD.ComputeHash(Encoding.ASCII.GetBytes(text));
		byte[] hash = mD.Hash;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			stringBuilder.Append(hash[i].ToString("x2"));
		}
		Console.WriteLine("md5 hash of they key=" + stringBuilder.ToString());
		return stringBuilder.ToString();
	}

	public static string TripleDESEncryptID(int input)
	{
		string stringToConvert = "a1b2c3d4e5";
		string stringToConvert2 = input.ToString();
		try
		{
			using TripleDES tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = StringToByte(stringToConvert, 24);
			tripleDES.IV = StringToByte("12345678");
			byte[] key = tripleDES.Key;
			byte[] iV = tripleDES.IV;
			ICryptoTransform transform = tripleDES.CreateEncryptor(key, iV);
			using MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(StringToByte(stringToConvert2), 0, StringToByte(stringToConvert2).Length);
			cryptoStream.FlushFinalBlock();
			byte[] buff = memoryStream.ToArray();
			return ByteToString(buff);
		}
		catch (Exception ex)
		{
			return ex.Message.ToString();
		}
	}

	public static int TripleDESDecryptID(string encryptedMessage)
	{
		string stringToConvert = "a1b2c3d4e5";
		int result = 0;
		try
		{
			if (encryptedMessage != "0")
			{
				using TripleDES tripleDES = new TripleDESCryptoServiceProvider();
				tripleDES.Key = StringToByte(stringToConvert, 24);
				tripleDES.IV = StringToByte("12345678");
				byte[] key = tripleDES.Key;
				byte[] iV = tripleDES.IV;
				ICryptoTransform transform = tripleDES.CreateDecryptor(key, iV);
				byte[] buffer = StringToByteDecimal(encryptedMessage);
				using MemoryStream stream = new MemoryStream(buffer);
				CryptoStream buff = new CryptoStream(stream, transform, CryptoStreamMode.Read);
				result = int.Parse(ByteToString(buff));
			}
			return result;
		}
		catch (Exception)
		{
			return result;
		}
	}

	public static byte[] StringToByteDecimal(string StringToConvert)
	{
		int num = 0;
		string text = "";
		byte[] array = new byte[StringToConvert.Length / 2];
		for (int i = 0; i < StringToConvert.Length; i += 2)
		{
			text = HexToDec(StringToConvert.Substring(i, 2));
			array[num] = Convert.ToByte(text);
			num++;
		}
		return array;
	}

	public static byte[] StringToByte(string StringToConvert)
	{
		char[] array = StringToConvert.ToCharArray();
		byte[] array2 = new byte[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = Convert.ToByte(array[i]);
		}
		return array2;
	}

	public static byte[] StringToByte(string StringToConvert, int length)
	{
		char[] array = StringToConvert.ToCharArray();
		byte[] array2 = new byte[length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = Convert.ToByte(array[i]);
		}
		return array2;
	}

	public static string ByteToString(CryptoStream buff)
	{
		string text = "";
		int num = 0;
		do
		{
			num = buff.ReadByte();
			if (num != -1)
			{
				text += (char)num;
			}
		}
		while (num != -1);
		return text;
	}

	public static string ByteToString(byte[] buff)
	{
		string text = "";
		for (int i = 0; i < buff.Length; i++)
		{
			text += buff[i].ToString("X2");
		}
		return text;
	}

	private static string HexToDec(string hexNum)
	{
		return Convert.ToString(int.Parse(hexNum, NumberStyles.HexNumber));
	}
}
