using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class NotificationDTO
	{
		public int id { get; set; }
		public string receivedBy { get; set; }
		public string sentBy { get; set; }
		public string description { get; set; }
		public string actionToken { get; set; }
		public DateTime sentTime { get; set; }
	}
}