using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("Competitions")]
	public class Competition
	{
		public Competition()
		{
			teams = new List<Team>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }

		[Required(ErrorMessage = "Competition name is required!")]
		[StringLength(20, ErrorMessage = "Competition name length must be between 5-20", MinimumLength = 5)]
		public string name { get; set; }

		[Required(ErrorMessage = "Competition description is required!")]
		public string description { get; set; }

		[Required(ErrorMessage = "Competition start date is required!")]
		public DateTime startDate { get; set; }

		[Required(ErrorMessage = "Competition end date is required!")]
		public DateTime endDate { get; set; }

		[ForeignKey("winnerID")]
		public virtual Team winner { get; set; }

		public int winnerID { get; set; }

		public  virtual ICollection<Team> teams { get; set; }
	}
}