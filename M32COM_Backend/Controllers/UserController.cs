using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace M32COM_Backend.Controllers
{
	public class UserController : ApiController
	{
		M32COMDBSERVER DB = new M32COMDBSERVER();

		
		public IEnumerable<User> Get()
		{

			return DB.Users.ToList();
		}

		public User Get(int id)
		{
			return DB.Users.FirstOrDefault(x => x.id == id);
		}

		public void Post(User user)
		{
			DB.Users.Add(user);
			DB.SaveChanges();
		}


	}
}
