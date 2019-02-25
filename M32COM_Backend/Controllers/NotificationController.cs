using M32COM_Backend.constants;
using M32COM_Backend.DTOs;
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
	[AuthorizationAttribute]
	[RoutePrefix("api/notification")]
	public class NotificationController : ApiController
    {
		[HttpGet]
		[Route("all")]
		public HttpResponseMessage getNotifications()
		{
			var token = Request.Headers.Authorization.Parameter;
			User user = UserUtility.GetUserByToken(token);
			using(M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				List<NotificationDTO> data = DB.Notifications.Where(x => x.isActive == true && x.receivedById == user.id).Select(x => new NotificationDTO { id = x.id, receivedById = x.receivedById, receivedByNameSurname = x.receivedBy.name + " " + x.receivedBy.surname, sentById = x.sentById, sentByNameSurname = x.sentBy.name + " " + x.sentBy.surname, actionToken = x.actionToken, description = x.description, sentTime = x.sentTime }).ToList();
				CustomResponse response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, data, ConstantResponse.NOTIFICATION_ALL_SUCCESS);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
			}
			
			
		}
    }
}
