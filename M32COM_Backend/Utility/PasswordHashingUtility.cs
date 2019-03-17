using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace M32COM_Backend.Utility
{
	public class PasswordHashingUtility
	{

		private static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

		public static string GenerateSaltedHash(string plainText,string salt)
		{
			HashAlgorithm algorithm = new SHA512Managed();

			string saltedPass = plainText + salt;


			return Convert.ToBase64String(algorithm.ComputeHash(Encoding.ASCII.GetBytes(saltedPass)));
		}

		public static string GenerateSalt()
		{
			const int saltLengthLimit = 32;

			var salt = new byte[saltLengthLimit];

			random.GetNonZeroBytes(salt);

			return Convert.ToBase64String(salt);
		}
	}
}