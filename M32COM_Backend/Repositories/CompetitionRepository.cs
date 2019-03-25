using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;

namespace M32COM_Backend.Repositories
{
	public class CompetitionRepository : ICompetitionRepository
	{
		private M32COMDBSERVER DB = new M32COMDBSERVER();

		public void Apply(int competitionId, int? teamId)
		{
			Team team = DB.Teams.Where(x => x.id == teamId).FirstOrDefault();
			Competition competition = GetById(competitionId);
			TeamCompetition teamComp = new TeamCompetition();

			teamComp.competition = competition;
			teamComp.competitionId = competition.id;
			teamComp.team = team;
			teamComp.teamId = team.id;
			teamComp.finishTime = CompetitionUtility.GetRandomDay(competition.startDate, competition.endDate);

			DB.TeamCompetitions.Add(teamComp);
			DB.SaveChanges();
		}

		public List<Competition> GetActiveCompetitions()
		{
			return DB.Competitions.Where(x => x.endDate > DateTime.Now && x.startDate < DateTime.Now).OrderByDescending(x => x.startDate).ToList();
		}

		public List<Competition> GetAllCompetitions()
		{
			return DB.Competitions.ToList();
		}

		public Competition GetById(int id)
		{
			return DB.Competitions.Where(x => x.id == id).FirstOrDefault();
		}

		public List<Competition> GetLastPassiveCompetitionsByRowCount(int rowCount)
		{
			return DB.Competitions.Where(x => x.endDate < DateTime.Now).OrderByDescending(d => d.startDate).Take(rowCount).ToList();
		}

		public List<TeamCompetition> GetResultByCompetitionId(int id)
		{
			return DB.TeamCompetitions.Where(x => x.competitionId == id).OrderBy(d => d.finishTime).ToList();
		}

		public ICollection<TeamCompetition> GetTeamsByCompetitionId(int id)
		{
			return GetById(id)?.teams;
		}

		public bool HasAlreadyApplied(int competitionId, int? teamId)
		{
			return DB.TeamCompetitions.Any(x => x.competitionId == competitionId && x.teamId == teamId);
		}
	}
}