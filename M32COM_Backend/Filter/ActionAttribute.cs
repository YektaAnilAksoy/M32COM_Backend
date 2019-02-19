using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace M32COM_Backend.Filter
{
	public class ActionAttribute : ActionFilterAttribute
	{
		// Before an Action Runs
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			using (M32COMDBSERVER context = new M32COMDBSERVER())
			{
				StringBuilder sb = new StringBuilder();
				foreach (var item in actionContext.ActionArguments)
				{
					sb.Append($"{item.Key}={item.Value.ToString()},");
				}
				context.Logs.Add(new Log()
				{
					isBefore = true,
					logCaption = $"{actionContext.ControllerContext.ControllerDescriptor.ControllerName} - {actionContext.ActionDescriptor.ActionName}",
					time = DateTime.Now,
					logDetail = sb.ToString()
				});
				context.SaveChanges();
			}
			base.OnActionExecuting(actionContext);
		}

		// After an Action Runs
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			using (M32COMDBSERVER context = new M32COMDBSERVER())
			{
				context.Logs.Add(new Log()
				{
					isBefore = false,
					logCaption = $"{actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName} - {actionExecutedContext.ActionContext.ActionDescriptor.ActionName}",
					time = DateTime.Now,
					logDetail = (actionExecutedContext.Response.Content as ObjectContent).ObjectType.FullName
				});
				context.SaveChanges();
			}
			base.OnActionExecuted(actionExecutedContext);
		}
	}
}