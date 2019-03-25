using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using M32COM_Backend.Models;
using M32COM_Backend.DTOs;

namespace M32COM_Backend.Repositories
{
	public class TeamRepository : ITeamRepository
	{
		public readonly IUserRepository _repository;
		private M32COMDBSERVER DB = new M32COMDBSERVER();

		//Dependency Injection
		public TeamRepository(IUserRepository repository)
		{
			_repository = repository;
		}


		public void AcceptTeamInvitation(string userEmail, int? teamId)
		{
			_repository.UpdateTeam(userEmail, teamId);
		}

		public Boat AddBoat(int leaderId, Boat boat)
		{
			Team team = GetTeamByLeaderId(leaderId);
			boat.teamId = team.id;
			team.boat = boat;
			DB.SaveChanges();

			return boat;
		}

		//Returns the team related with the given Leader Id
		public Team GetTeamByLeaderId(int leaderId)
		{
			return DB.Teams.Include(x=>x.boat).Where(x => x.leaderId == leaderId).FirstOrDefault();
		}

		//Returns All members of the team related with the given team Id
		public IEnumerable<UserDTO> GetTeamMembers(int? teamId)
		{
			return DB.Users.Include(x => x.team).Where(x => x.team.id == teamId).Select(x => new UserDTO { id = x.id, name = x.name, surname = x.surname, email = x.email, teamName = x.team.name }).ToList();
		}

		//Creates the team and make the user having the email leader.
		public Team InsertTeam(string email, Team team)
		{
			User user = DB.Users.Include(x => x.team).First(x=>x.email == email);
			team.leaderId = user.id;
			team.teamMembers.Add(user);
			DB.Teams.Add(team);
			DB.SaveChanges();
			return team;
		}

		//Returns true if the team name is unique
		public bool IsTeamNameUnique(string teamName)
		{
			return !(DB.Teams.Any(x => x.name.Equals(teamName,StringComparison.OrdinalIgnoreCase)));
		}

		//Returns true if the user is a team leader
		public bool IsUserLeader(string email)
		{
			int userId = DB.Users.Where(x => x.email == email).Select(x => x.id).SingleOrDefault();
			var isUserLeader = DB.Teams.Any(x => x.leaderId == userId);

			return isUserLeader;
		}

		//Returns true if the user has a team
		public bool UserHasTeam(string email)
		{
			var team = DB.Users.Include(x => x.team).Where(x=>x.email == email).Select(x => x.team).SingleOrDefault();

			return team == null ? false : true;
		}

	}
}