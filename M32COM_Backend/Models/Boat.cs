using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("Boats")]
	public class Boat
	{
		
		[Key, ForeignKey("team")]
		public int id { get; set; }

		[Required(ErrorMessage = "Model is required!")]
		[StringLength(20, ErrorMessage = "Model length must be between 3-20", MinimumLength = 3)]
		public string model { get; set; }


		[Required(ErrorMessage = "Year is required!")]
		public int year { get; set; }

		public int teamId { get; set; }
		public virtual Team team { get; set; }

	}
}