using M32COM_Backend.Models;
using M32COM_Backend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace M32COM_Backend.Filter
{
	public class ErrorAttribute:ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			CustomResponse response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, actionExecutedContext.Exception.ToString());

			//Loging
			Logger.Log(actionExecutedContext.Exception.Message,actionExecutedContext.Exception.StackTrace);


			//Creating Response Object
			//ErrorResponse response = new ErrorResponse();
			//response.errorAction = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
			//response.errorController = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
			//response.errorMessage = actionExecutedContext.Exception.ToString();

			//Adding response object to response
			actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, response);
		}
	}
}