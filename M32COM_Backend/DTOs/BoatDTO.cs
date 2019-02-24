using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class BoatDTO
	{
		public int id { get; set; }
		public string model { get; set; }
		public int year { get; set; }
		public string teamName { get; set; }
	}
}