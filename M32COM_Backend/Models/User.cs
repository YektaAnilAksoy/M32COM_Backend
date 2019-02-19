using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("Users")]
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }

		[Required]
		public string name { get; set; }

		[Required]
		public string surname { get; set; }

		[Required]
		public string email { get; set; }

		[Required]
		public string password { get; set; }

		public Team team { get; set; }
	}
}