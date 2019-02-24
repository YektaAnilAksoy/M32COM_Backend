using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class UserDTO
	{
		public int id { get; set; }
		public string name { get; set; }
		public string surname { get; set; }
		public string email { get; set; }
		public string teamName { get; set; }
		
	}
}