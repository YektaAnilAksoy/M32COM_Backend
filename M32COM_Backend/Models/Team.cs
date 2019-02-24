using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static M32COM_Backend.Utility.CustomModelValidation;

namespace M32COM_Backend.Models
{
	[Table("Teams")]
	public class Team
	{
		public Team()
		{
			teamMembers = new List<User>();
			Competitions = new List<Competition>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }


		[Required(ErrorMessage = "Team name is required!")]
		[StringLength(20, ErrorMessage = "Team name length must be between 3-20", MinimumLength = 3)]
		//[UniqueTeamName(ErrorMessage = "This team name is already in use !")]
		public string name { get; set; }

		
		public int leaderId { get; set; }

		public virtual Boat boat { get; set; }

		public virtual ICollection<User> teamMembers { get; set; }
		public virtual ICollection<Competition> Competitions { get; set; }
	}
}