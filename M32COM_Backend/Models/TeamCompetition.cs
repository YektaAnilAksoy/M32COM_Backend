using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("TeamCompetition")]
	public class TeamCompetition
	{
		[Key,Column(Order = 0)]
		[ForeignKey("team")]
		public int teamId { get; set; }

		[Key,Column(Order = 1)]
		[ForeignKey("competition")]
		public int competitionId { get; set; }

		public DateTime finishTime { get; set; }

		public virtual Team team { get; set; }
		public virtual Competition competition { get; set; }
	}
}