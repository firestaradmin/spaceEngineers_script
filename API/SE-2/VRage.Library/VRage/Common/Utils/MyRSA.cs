using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VRage.Cryptography;

namespace VRage.Common.Utils
{
	public class MyRSA
	{
		private HashAlgorithm m_hasher;

		public HashAlgorithm HashObject => m_hasher;

		public MyRSA()
		{
			m_hasher = (HashAlgorithm)(object)MySHA256.Create();
			m_hasher.Initialize();
		}

		public void GenerateKeys(string publicKeyFileName, string privateKeyFileName)
		{
			GenerateKeys(out var publicKey, out var privateKey);
			if (publicKey != null && privateKey != null)
			{
				File.WriteAllText(publicKeyFileName, Convert.ToBase64String(publicKey));
				File.WriteAllText(privateKeyFileName, Convert.ToBase64String(privateKey));
			}
		}

		/// <summary>
		/// Generate keys into specified files.
		/// </summary>
		/// <param name="publicKey">Name of the file that will contain public key</param>
		/// <param name="privateKey">Name of the file that will contain private key</param>
		public void GenerateKeys(out byte[] publicKey, out byte[] privateKey)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			CspParameters val = null;
			RSACryptoServiceProvider val2 = null;
			try
			{
				CspParameters val3 = new CspParameters
				{
					ProviderType = 1
				};
				val3.set_Flags((CspProviderFlags)16);
				val3.KeyNumber = 1;
				val = val3;
				val2 = new RSACryptoServiceProvider(val);
				val2.set_PersistKeyInCsp(false);
				publicKey = val2.ExportCspBlob(false);
				privateKey = val2.ExportCspBlob(true);
			}
			catch (Exception)
			{
				publicKey = null;
				privateKey = null;
			}
			finally
			{
				if (val2 != null)
				{
					val2.set_PersistKeyInCsp(false);
				}
			}
		}

		/// <summary>
		/// Signs given data with provided key.
		/// </summary>
		/// <param name="data">data to sign (in base64 form)</param>
		/// <param name="privateKey">private key (in base64 form)</param>
		/// <returns>Signed data (string in base64 form)</returns>
		public string SignData(string data, string privateKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RSACryptoServiceProvider val = new RSACryptoServiceProvider();
			byte[] inArray;
			try
			{
				val.set_PersistKeyInCsp(false);
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(data);
				try
				{
					val.ImportCspBlob(Convert.FromBase64String(privateKey));
					inArray = val.SignData(bytes, (object)m_hasher);
				}
				catch (CryptographicException)
				{
					return null;
				}
				finally
				{
					val.set_PersistKeyInCsp(false);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// Signs given hash with provided key.
		/// </summary>
		/// <param name="hash">hash to sign</param>
		/// <param name="privateKey">private key</param>
		/// <returns>Signed hash (string in base64 form)</returns>
		public string SignHash(byte[] hash, byte[] privateKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RSACryptoServiceProvider val = new RSACryptoServiceProvider();
			byte[] inArray;
			try
			{
				val.set_PersistKeyInCsp(false);
				try
				{
					val.ImportCspBlob(privateKey);
					inArray = val.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));
				}
				catch (CryptographicException)
				{
					return null;
				}
				finally
				{
					val.set_PersistKeyInCsp(false);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// Signs given hash with provided key.
		/// </summary>
		/// <param name="hash">hash to sign (in base64 form)</param>
		/// <param name="privateKey">private key (in base64 form)</param>
		/// <returns>Signed hash (string in base64 form)</returns>
		public string SignHash(string hash, string privateKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RSACryptoServiceProvider val = new RSACryptoServiceProvider();
			byte[] inArray;
			try
			{
				val.set_PersistKeyInCsp(false);
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(hash);
				try
				{
					val.ImportCspBlob(Convert.FromBase64String(privateKey));
					inArray = val.SignHash(bytes, CryptoConfig.MapNameToOID("SHA256"));
				}
				catch (CryptographicException)
				{
					return null;
				}
				finally
				{
					val.set_PersistKeyInCsp(false);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// Verifies that a digital signature is valid by determining the hash value
		/// in the signature using the provided public key and comparing it to the provided hash value.
		/// </summary>
		/// <param name="hash">hash to test</param>
		/// <param name="signedHash">already signed hash</param>
		/// <param name="publicKey">signature</param>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		public bool VerifyHash(byte[] hash, byte[] signedHash, byte[] publicKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RSACryptoServiceProvider val = new RSACryptoServiceProvider();
			try
			{
				try
				{
					val.ImportCspBlob(publicKey);
					return val.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signedHash);
				}
				catch (CryptographicException)
				{
					return false;
				}
				finally
				{
					val.set_PersistKeyInCsp(false);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		/// <summary>
		/// Verifies that a digital signature is valid by determining the hash value
		/// in the signature using the provided public key and comparing it to the provided hash value.
		/// </summary>
		/// <param name="hash">hash to test</param>
		/// <param name="signedHash">already signed hash (in base64 form)</param>
		/// <param name="publicKey">signature (in base64 form)</param>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		public bool VerifyHash(string hash, string signedHash, string publicKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RSACryptoServiceProvider val = new RSACryptoServiceProvider();
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(hash);
				byte[] array = Convert.FromBase64String(signedHash);
				try
				{
					val.ImportCspBlob(Convert.FromBase64String(publicKey));
					return val.VerifyHash(bytes, CryptoConfig.MapNameToOID("SHA256"), array);
				}
				catch (CryptographicException)
				{
					return false;
				}
				finally
				{
					val.set_PersistKeyInCsp(false);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		/// <summary>
		/// Verifies that a digital signature is valid by determining the hash value
		/// in the signature using the provided public key and comparing it to the hash value of the provided data.
		/// </summary>
		/// <param name="originalMessage">original data</param>
		/// <param name="signedMessage">signed message (in base64 form)</param>
		/// <param name="publicKey">signature (in base64 form)</param>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		public bool VerifyData(string originalMessage, string signedMessage, string publicKey)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RSACryptoServiceProvider val = new RSACryptoServiceProvider();
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(originalMessage);
				byte[] array = Convert.FromBase64String(signedMessage);
				try
				{
					val.ImportCspBlob(Convert.FromBase64String(publicKey));
					return val.VerifyData(bytes, (object)m_hasher, array);
				}
				catch (CryptographicException)
				{
					return false;
				}
				finally
				{
					val.set_PersistKeyInCsp(false);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
