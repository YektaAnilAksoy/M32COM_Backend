using M32COM_Backend.Controllers;
using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace M32COM_Backend.StartupClasses
{
	public static class InitialDataLoader
	{
		/*private static M32COMDBSERVER DB = new M32COMDBSERVER();


		public static void LoadAllDatas()
		{
			LoadInitialUsers();
		}

		private static void LoadInitialUsers()
		{
			var controller = new RegistrationController();

			User user1 = new User
			{
				name = "Red",
				surname = "Dragon",
				email ="reddragon@testmail.com",
				password = "123456"
			};

			User user2 = new User
			{
				name = "Green",
				surname = "Arrow",
				email = "greenarrow@testmail.com",
				password = "123456"
			};

			User user3 = new User
			{
				name = "Black",
				surname = "Panther",
				email = "blackpanther@testmail.com",
				password = "123456"
			};

			User user4 = new User
			{
				name = "White",
				surname = "Tiger",
				email = "whitetiger@testmail.com",
				password = "123456"
			};

			User user5 = new User
			{
				name = "Yellow",
				surname = "Jacket",
				email = "yellowjacket@testmail.com",
				password = "123456"
			};

			controller.Request = new HttpRequestMessage();
			controller.Configuration = new HttpConfiguration();

			controller.Register(user1);
			controller.Register(user2);
			controller.Register(user3);
			controller.Register(user4);
			controller.Register(user5);
		}*/
	}
}