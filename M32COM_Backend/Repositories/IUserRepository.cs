using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M32COM_Backend.Repositories
{
	public interface IUserRepository
	{
		User GetByEmail(string email);
		User GetById(int id);
		void UpdateTeam(string email, int? teamId);
		List<User> GetAll();
	}
}
