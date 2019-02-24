using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Models
{
	[Table("Notifications")]
	public class Notification
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }

		[Required]
		public int  sentBy { get; set; }

		[Required]
		public  User receivedBy { get; set; }

		public int receivedById { get; set; }

		[Required]
		public string description { get; set; }

		[Required]
		public string actionToken { get; set; }

		[Required]
		public Boolean isActive { get; set; }

		[Required]
		public DateTime sentTime { get; set; }

	}
}