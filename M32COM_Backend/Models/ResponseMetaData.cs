using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace M32COM_Backend.Models
{
	public class ResponseMetadata
	{
		
		public HttpStatusCode statusCode { get; set; }
		public bool hasError { get; set; }
		public string message { get; set; }
		public DateTime timestamp { get; set; }
	}
}