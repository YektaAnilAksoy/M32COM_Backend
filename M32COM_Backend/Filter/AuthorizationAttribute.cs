﻿using M32COM_Backend.Models;
using M32COM_Backend.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace M32COM_Backend.Filter
{
	public class AuthorizationAttribute :AuthorizationFilterAttribute
	{
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if(actionContext.Request.Headers.Authorization == null)
			{
				actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
			}
			else
			{
				UserToken loginUser = null;
				try
				{
					//Gets token key from header
					var tokenKey = actionContext.Request.Headers.Authorization.Parameter;

					//Decrypts token key 
					var jsonString = FTH.Extension.Encrypter.Decrypt(tokenKey, LoginUtility.PRIVATE_KEY);

					//Deserializes json string to get user object
					loginUser = JsonConvert.DeserializeObject<UserToken>(jsonString);

					#region - Old Basic Authorization -
					//Converting token key base64 to string and encode it as UTF8
					//var emailPassword = Encoding.UTF8.GetString(Convert.FromBase64String(tokenKey));
					//var userInfoArray = emailPassword.Split(':');
					//var email = userInfoArray[0];
					//var password = userInfoArray[1];
					#endregion
				}
				catch
				{
					actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
					return;
				}


				if (LoginUtility.EmailAndPassword(loginUser.email, loginUser.password) && DateTime.Compare(DateTime.Now, loginUser.expireDate) < 0)
				{
					Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(loginUser.email), null);
				}
				else
				{
					actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
				}
			}
		}
	}
}