using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;

namespace M32COM_Backend.Repositories
{
	public class LoginRepository : ILoginRepository
	{
		private M32COMDBSERVER DB = new M32COMDBSERVER();

		public User Login(LoginModel model)
		{
			var userSalt = DB.Users.Where(u => u.email == model.email).Select(u => u.passwordSalt).SingleOrDefault();

			if (userSalt == null)
				return null;
		
			string saltedPass = PasswordHashingUtility.GenerateSaltedHash(model.password, userSalt);

			return DB.Users.Include(x => x.receivedNotification).Include(x => x.team).Where(x => x.email.Equals(model.email, StringComparison.OrdinalIgnoreCase) && x.password == saltedPass).FirstOrDefault();
		}
	}
}