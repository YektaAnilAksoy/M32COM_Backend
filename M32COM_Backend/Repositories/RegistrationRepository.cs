using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.Attributes;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;

namespace M32COM_Backend.Repositories
{
	public class RegistrationRepository : IRegistrationRepository
	{

		private M32COMDBSERVER DB = new M32COMDBSERVER();

		//Inserts user if the user's email is not in use.
		public User Register(User user)
		{

			//Random salt is created
			var salt = PasswordHashingUtility.GenerateSalt();

			//Hash is created by using random salt and password
			user.password = PasswordHashingUtility.GenerateSaltedHash(user.password, salt);
			user.passwordSalt = salt;

			if (DB.Users.Any(x => x.email == user.email))
				return null;

			DB.Users.Add(user);
			DB.SaveChanges();
			return user;
		}
	}
}