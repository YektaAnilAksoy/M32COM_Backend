using M32COM_Backend.constants;
using M32COM_Backend.DTOs;
using M32COM_Backend.Filter;
using M32COM_Backend.Mappers;
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
	//[AuthorizationAttribute]
	public class LoginController : ApiController
    {
		private const int TOKEN_EXPIRE_DAY = 1;

		[HttpPost]
		public HttpResponseMessage Login([FromBody] LoginModel loginModel)
		{

			string email = loginModel.email;
			string password = loginModel.password;
			CustomResponse response;
			User loginUser = LoginUtility.GetUserByEmailAndPassword(email, password);
			if (loginUser != null)
			{
				//LoginUser
				var userToken = new UserTokenDTO()
				{
					email = email,
					password = password,
					expireDate = DateTime.Now.AddDays(TOKEN_EXPIRE_DAY)
				};

				//Serializing userToken
				var jsonString = JsonConvert.SerializeObject(userToken);
				// Token generation
				var token = FTH.Extension.Encrypter.Encrypt(jsonString, LoginUtility.PRIVATE_KEY);

				LoginResponseDTO userDTO = GenericMapper.MapToLoginResponseDTO(loginUser, token);

				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, userDTO, ConstantResponse.LOGIN_SUCCESS);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
			}
			else
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Unauthorized, true, null, ConstantResponse.LOGIN_FAILED);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.Unauthorized, response);
			}
		}

    }
}
