﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	public class UserToken
	{
		public string email { get; set; }
		public string password { get; set; }
		public DateTime expireDate { get; set; }
	}
}