using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class CompetitionDTO
	{
		public CompetitionDTO()
		{
			teams = new List<TeamDTO>();
		}

		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public ICollection<TeamDTO> teams { get; set; }
	}
}