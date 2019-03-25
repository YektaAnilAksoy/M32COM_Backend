using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M32COM_Backend.Repositories
{
	public interface ILoginRepository
	{
		User Login(LoginModel model);
	}
}
