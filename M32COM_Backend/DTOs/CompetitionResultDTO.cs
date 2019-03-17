using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class CompetitionResultDTO
	{
		public CompetitionResultDTO()
		{
			teams = new List<TeamResultDTO>();
		}

		public CompetitionDTO competition { get; set; }
		public ICollection<TeamResultDTO> teams { get; set; }
	}
}