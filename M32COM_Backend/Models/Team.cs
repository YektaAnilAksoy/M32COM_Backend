using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("Teams")]
	public class Team
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }

		[Required]
		public string name { get; set; }
	}
}