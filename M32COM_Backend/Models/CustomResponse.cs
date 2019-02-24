using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	public class CustomResponse : ResponseMetadata
	{
		public Object result { get; set; }
	}
}