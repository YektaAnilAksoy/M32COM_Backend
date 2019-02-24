using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static M32COM_Backend.Utility.CustomModelValidation;

namespace M32COM_Backend.Models
{
	[Table("Users")]
	public class User
	{
		public User()
		{
			receivedNotification = new List<Notification>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int id { get; set; }

		[Required(ErrorMessage = "Name is required!")]
		[StringLength(20, ErrorMessage = "Name length must be between 3-20", MinimumLength = 3)]
		public string name { get; set; }

		[Required(ErrorMessage = "Surname is required!")]
		[StringLength(20, ErrorMessage = "Surname length must be between 3-20", MinimumLength = 3)]
		public string surname { get; set; }

		[Required(ErrorMessage = "Email is required!")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string email { get; set; }

		[Required(ErrorMessage = "Password is required!")]
		[StringLength(20, ErrorMessage = "Password length must be between 6-20",MinimumLength = 6)]
		public string password { get; set; }


		public virtual Team team { get; set; }

		public int? teamId { get; set; }

		public virtual ICollection<Notification> receivedNotification { get; set; }
	}
}