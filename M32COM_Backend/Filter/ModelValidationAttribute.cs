using M32COM_Backend.Models;
using M32COM_Backend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace M32COM_Backend.Filter
{
	public class ModelValidationAttribute:ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			//if it is valid, it will continue
			if(actionContext.ModelState.IsValid)
				base.OnActionExecuting(actionContext);

			//if it is not, errors are selected and written into response message
			else
			{
				var errorList = actionContext.ModelState.Values.SelectMany(v => v.Errors)
									.Select(x => x.ErrorMessage).ToList();
				var errorMessage = string.Join(Environment.NewLine, errorList);

				CustomResponse response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, errorMessage);
				actionContext.Response =  actionContext.Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}
		}
	}
}