using M32COM_Backend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.DTOs
{
	public class TeamDTO
	{
		public TeamDTO()
		{
			teamMembers = new List<UserDTO>();
		}

		public int id { get; set; }
		public int leaderId { get; set; }
		public string name { get; set; }
		public ICollection<UserDTO> teamMembers { get; set; }
	}
}