using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Utility
{
	public static class CompetitionUtility
	{
		private static readonly Random rnd = new Random();
		public static DateTime  GetRandomDay(DateTime start,DateTime end)
		{
			var range = end - start;
			var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));
			return (start + randTimeSpan);
		}
	}
}