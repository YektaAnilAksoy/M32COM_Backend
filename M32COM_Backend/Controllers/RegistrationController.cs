using M32COM_Backend.constants;
using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;
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
	[ModelValidationAttribute]
	public class RegistrationController : ApiController
	{
		[HttpPost]
		public HttpResponseMessage Register([FromBody] User user)
		{
			CustomResponse response;

			using(M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				if (UserUtility.IsEmailUnique(user.email)){
					DB.Users.Add(user);
					DB.SaveChanges();
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Created, false, ConstantResponse.OK, ConstantResponse.USER_CREATED);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.Created, response);

				}
				//Email in use
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.EMAIL_IN_USE);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);

			}
		}
	}
}