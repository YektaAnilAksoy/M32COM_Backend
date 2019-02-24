using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Utility
{
	public static class TeamUtility
	{
		public static bool  IsTeamNameUnique(string teamname)
		{
			using(M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				return !(DB.Teams.Any(x => x.name == teamname));
			}
		}
	}
}