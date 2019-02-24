using M32COM_Backend.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Utility
{
	[ErrorAttribute]
	public class CustomModelValidation
	{
		////Checks uniqueness of given Email
		//public sealed class UniqueEmail : ValidationAttribute
		//{
		//	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		//	{
		//		string email = value.ToString();
		//		using(M32COMDBSERVER DB = new M32COMDBSERVER())
		//		{
		//			// email in use !
		//			if (DB.Users.Any(x => x.email == email))
		//				return new ValidationResult("This email is already in use !");

		//			// email is free!
		//			return ValidationResult.Success;
		//		}
		//	}
		//}


		//Checks uniqueness of given team name
		//public sealed class UniqueTeamName : ValidationAttribute
		//{
		//	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		//	{
		//		string teamName = value.ToString();
		//		using(M32COMDBSERVER DB = new M32COMDBSERVER())
		//		{
		//			//Team name in use!
		//			if(DB.Teams.Any(x=>x.name == teamName))
		//				return new ValidationResult("This team name is already in use !");

		//			return ValidationResult.Success;
		//		}
		//	}
		//}

		
	}
}