using M32COM_Backend.constants;
using M32COM_Backend.Filter;
using M32COM_Backend.Mappers;
using M32COM_Backend.Models;
using M32COM_Backend.Repositories;
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
		//Dependency injection
		public readonly IRegistrationRepository _repository;

		public RegistrationController(IRegistrationRepository repository)
		{
			_repository = repository;
		}

		[HttpPost]
		public HttpResponseMessage Register([FromBody] User user)
		{
			CustomResponse response;
			//Inserting the user into DB
			var result = _repository.Register(user);

			//If result is not null, it means that the given email is unique!
			if(result != null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Created, false, GenericMapper.MapToUserDTO(result), ConstantResponse.USER_CREATED);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.Created, response);
			}

			//Email in use
			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.EMAIL_IN_USE);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
		}
	}
}