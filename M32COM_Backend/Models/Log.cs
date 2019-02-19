using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("Logs")]
	public class Log
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }

		public DateTime time { get; set; }

		[MaxLength(300)]
		public string logCaption { get; set; }

		public string logDetail { get; set; }

		public bool isBefore { get; set; }
	}
}