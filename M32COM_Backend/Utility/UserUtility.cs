using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using M32COM_Backend.DTOs;

namespace M32COM_Backend.Utility
{
	[ErrorAttribute]
	public static class UserUtility
	{
		public static Boolean IsEmailUnique(string email)
		{
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				// email in use !
				if (DB.Users.Any(x => x.email == email))
					return false;

				// email is free!
				return true;
			}
		}

		public static User GetUserByToken(string token)
		{
			string email = GetEmailByToken(token);

			return GetUserByEmail(email);
		}

		public static string GetEmailByToken(string token)
		{
			//Decrypts token key 
			var jsonString = FTH.Extension.Encrypter.Decrypt(token, LoginUtility.PRIVATE_KEY);

			//Deserializes json string to get UserToken object
			UserTokenDTO loginUser = JsonConvert.DeserializeObject<UserTokenDTO>(jsonString);

			return loginUser.email;
		}

		public static User GetUserByEmail(string email)
		{
			using(M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				return DB.Users.Include(x=>x.receivedNotification).Include(x=>x.sentNotification).Include(x=>x.team).Include(x=>x.team.boat).First(x => x.email == email);
			}
		}

		public static User GetUserById(int id)
		{
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				return DB.Users.Include(b => b.team).Include(x => x.team.boat).Where(x => x.id == id).First();
			}
		}

		public static Team GetUserTeamById(int id)
		{
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				return DB.Users.Include(b => b.team).Include(x => x.team.boat).Where(y=>y.id==id).Select(x=>x.team).SingleOrDefault();
			}
		}

	}
}