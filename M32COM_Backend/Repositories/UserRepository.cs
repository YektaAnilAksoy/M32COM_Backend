using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using M32COM_Backend.Models;

namespace M32COM_Backend.Repositories
{
	public class UserRepository : IUserRepository
	{
		private M32COMDBSERVER DB = new M32COMDBSERVER();

		public List<User> GetAll()
		{
			return DB.Users.ToList();
		}

		public User GetByEmail(string email)
		{
			return DB.Users.Include(x=>x.team).Include(x => x.team.boat).Include(x=>x.receivedNotification).FirstOrDefault(x => x.email == email);
		}

		public User GetById(int id)
		{
			return DB.Users.Include(x=>x.team).Include(x => x.team.boat).Include(x=>x.receivedNotification).FirstOrDefault(x => x.id == id);
		}

		
		
		public void UpdateTeam(string email, int? teamId)
		{
			User user = GetByEmail(email);
			user.teamId = teamId;
			DB.Users.Attach(user);
			DB.Entry(user).Property(x => x.teamId).IsModified = true;
			DB.SaveChanges();
		}
	}
}