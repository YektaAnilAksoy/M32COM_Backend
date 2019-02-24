using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class LoginResponseDTO :UserDTO
	{
		public string token { get; set; }
	}
}