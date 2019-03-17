using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace M32COM_Backend.Utility
{
	[ErrorAttribute]
	public class LoginUtility
	{
		//Used for token encryption and decryption
		internal const string  PRIVATE_KEY = "159357";

		public static User GetUserByEmailAndPassword(string email, string password)
		{
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				var userSalt = DB.Users.Where(u => u.email == email).Select(u => u.passwordSalt).SingleOrDefault();

				if (userSalt == null)
					return null;

				string saltedPass = PasswordHashingUtility.GenerateSaltedHash(password,userSalt);

				return DB.Users.Include(x=>x.receivedNotification).Include(x=>x.team).Where(x => x.email.Equals(email, StringComparison.OrdinalIgnoreCase) && x.password == saltedPass).FirstOrDefault();
			}
		}
	}
}