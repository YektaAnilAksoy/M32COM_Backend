using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using M32COM_Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace M32COM_Backend.Controllers
{
	[ErrorAttribute]
	[ActionAttribute]
	[AuthorizationAttribute]
	public class UserController : ApiController
	{
		IUserRepository _repository;

		public UserController(IUserRepository repository)
		{
			_repository = repository;
		}

		

		public User GetById(int id)
		{
			return _repository.GetById(id);
		}

		public User GetByEmail(string email)
		{
			return _repository.GetByEmail(email);
		}
	}
}
