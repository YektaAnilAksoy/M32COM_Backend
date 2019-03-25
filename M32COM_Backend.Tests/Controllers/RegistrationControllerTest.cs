using M32COM_Backend.Controllers;
using M32COM_Backend.Models;
using M32COM_Backend.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace M32COM_Backend.Tests.Controllers
{
	[TestClass]
	class RegistrationControllerTest
	{

		/// <summary>
		/// Our Mock Registration Repository for use in testing
		/// </summary>
		public readonly IRegistrationRepository MockRegistrationRepository;
		public readonly IUserRepository MockUserRepository;

		public RegistrationControllerTest()
		{
			// create some mock products to play with
			List<User> users = new List<User>
				{
					new User
							{
								name = "testUserName2",
								surname = "testUserSurname2",
								email = "testuser1@testmail.com",
								password = "123456"
							},
					new User
							{
								name = "testUserName2",
								surname = "testUserSurname2",
								email = "testuser2@testmail.com",
								password = "123456"
							},
					new User
							{
								name = "testUserName3",
								surname = "testUserSurname",
								email = "testuser3@testmail.com",
								password = "123456"
							}
				};

			// Mock the Registration Repository using Moq
			Mock<IRegistrationRepository> registrationRepository = new Mock<IRegistrationRepository>();
			registrationRepository.Setup(x => x.Register(It.IsAny<User>())).Returns<User>(x => x);

			// Allows us to test saving a product
			registrationRepository.Setup(r => r.Register(It.IsAny<User>())).Returns(
				(User target) =>
				{
					users.Add(target);
					return target;
				});

			Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
			//Returns all users
			userRepository.Setup(x => x.GetAll()).Returns(users);

			// Complete the setup of our Mock Product Repository
			this.MockRegistrationRepository = registrationRepository.Object;
			this.MockUserRepository = userRepository.Object;
		}



		[TestMethod]
		public void Register_Positive()
		{
			var repository = new Mock<IRegistrationRepository>();
			
			User testUser4 = new User
			{
				name = "testUserName4",
				surname = "testUserSurname4",
				email = "testuser4@testmail.com",
				password = "123456"
			};

			int userCount = this.MockUserRepository.GetAll().Count;
			Assert.AreEqual(3, userCount); // Verify the expected Number pre-insert

			// try saving our new product
			this.MockRegistrationRepository.Register(testUser4);

			userCount = this.MockUserRepository.GetAll().Count;
			Assert.AreEqual(4, userCount); // Verify the expected Number pre-insert
		}

		[TestMethod]
		public void Correct()
		{
			Assert.AreEqual(4, 4);
		}
	}
}
