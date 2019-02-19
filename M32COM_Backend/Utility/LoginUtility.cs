using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Utility
{
	public class LoginUtility
	{
		internal const string  PRIVATE_KEY = "159357";

		public static bool EmailAndPassword(string email, string password)
		{
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				return DB.Users.Any(x => x.email.Equals(email, StringComparison.OrdinalIgnoreCase) && x.password == password);
			}
		}
	}
}