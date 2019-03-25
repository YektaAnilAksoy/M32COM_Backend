using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M32COM_Backend.Repositories
{
	public interface ICompetitionRepository
	{
		Competition GetById(int id);
		ICollection<TeamCompetition> GetTeamsByCompetitionId(int id);
		List<Competition> GetAllCompetitions();
		List<Competition> GetActiveCompetitions();
		List<Competition> GetLastPassiveCompetitionsByRowCount(int rowCount);
		bool HasAlreadyApplied(int competitionId,int? teamId);
		void Apply(int competitionId, int? teamId);
		List<TeamCompetition> GetResultByCompetitionId(int id);
	}
}
