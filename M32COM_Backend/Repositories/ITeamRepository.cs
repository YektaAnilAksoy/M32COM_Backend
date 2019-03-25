using M32COM_Backend.DTOs;
using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M32COM_Backend.Repositories
{
	public interface ITeamRepository
	{
		bool IsTeamNameUnique(string teamName);
		bool UserHasTeam(string email);
		bool IsUserLeader(string email);
		Team InsertTeam(string email, Team team);
		void AcceptTeamInvitation(string userEmail, int? teamId);
		Boat AddBoat(int leaderId, Boat boat);
		Team GetTeamByLeaderId(int leaderId);
		IEnumerable<UserDTO> GetTeamMembers(int? teamId);
	}
}
