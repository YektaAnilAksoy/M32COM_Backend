using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class TeamResultDTO
	{
		public TeamResultDTO()
		{
			members = new List<String>();
		}

		public string name { get; set; }
		public ICollection<String> members;
		public DateTime finishTime { get; set; }
	}
}