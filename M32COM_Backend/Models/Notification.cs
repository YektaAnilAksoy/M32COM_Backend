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

		
		public virtual User  sentBy { get; set; }

		public  virtual User receivedBy { get; set; }

		[ForeignKey("receivedBy")]
		public int? receivedById { get; set; }

		[ForeignKey("sentBy")]
		public int? sentById { get; set; }

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