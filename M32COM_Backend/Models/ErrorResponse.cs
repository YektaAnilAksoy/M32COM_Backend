using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Filter
{
	public class ErrorResponse
	{
		public string errorMessage { get; set; };
		public string errorAction { get; set; };
		public string errorController { get; set; };
	}
}