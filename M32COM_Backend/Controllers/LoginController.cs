using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace M32COM_Backend.Controllers
{
	[ErrorAttribute]
	[ActionAttribute]
	[AuthorizationAttribute]
    public class LoginController : ApiController
    {

		[HttpGet]
		public HttpResponseMessage Login([FromBody] string email, [FromBody] string password)
		{
			// return Thread.CurrentPrincipal.Identity.Name;

			if (LoginUtility.EmailAndPassword(email, password))
			{
				//LoginUser
				var userToken = new UserToken()
				{
					email = email,
					password = password,
					expireDate = DateTime.Now.AddDays(1)
				};

				//Serializing user object
				var jsonString = JsonConvert.SerializeObject(userToken);

				// Token generation
				var token = FTH.Extension.Encrypter.Encrypt(jsonString, LoginUtility.PRIVATE_KEY);

				return Request.CreateResponse(HttpStatusCode.OK, token);
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Email or password is wrong!");
			}
		}

    }
}
