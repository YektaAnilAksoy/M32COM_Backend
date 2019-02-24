using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace M32COM_Backend.Utility
{
	public static class ResponseMessageHelper
	{
		public static CustomResponse CreateResponse(HttpStatusCode statusCode,bool hasError,Object data,string message)
		{
			CustomResponse response = new CustomResponse();
			response.result = data;
			response.hasError = hasError;
			response.statusCode = statusCode;
			
			SetMetaData(response,message);
			return response;
		}
		private static void SetMetaData(ResponseMetadata response,string message)
		{
			response.timestamp = DateTime.Now;
			response.message = message;
		}
	}
}