using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using M32COM_Backend.constants;
using M32COM_Backend.DTOs;
using M32COM_Backend.Mappers;
using M32COM_Backend.Repositories;

namespace M32COM_Backend.Controllers
{
	[ErrorAttribute]
	[ActionAttribute]
	[ModelValidationAttribute]
	[AuthorizationAttribute]
	[RoutePrefix("api/team")]
	public class TeamController : ApiController
	{
		ITeamRepository _repository;
		IUserRepository _userRepository;
		INotificationRepository _notificationRepository;
		ICompetitionRepository _competitionRepository;
		//Depedency Injection
		public TeamController(ITeamRepository repository, IUserRepository userRepository, INotificationRepository notificationRepository, ICompetitionRepository competitionRepository)
		{
			_repository = repository;
			_userRepository = userRepository;
			_notificationRepository = notificationRepository;
			_competitionRepository = competitionRepository;
		}

		[HttpPost]
		[Route("create")]
		public HttpResponseMessage Create([FromBody] Team team)
		{
			//Gets the user from token
			var token = Request.Headers.Authorization.Parameter;

			string userEmail = UserUtility.GetEmailByToken(token);

			CustomResponse response;
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				//Returns error if team name is not unique
				if (!_repository.IsTeamNameUnique(team.name))
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_FAILED);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);

				}

				//Returns error if the user has already had a team
				bool userHasTeam = _repository.UserHasTeam(userEmail);
				if (userHasTeam)
				{

					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest,true,null, ConstantResponse.HAS_TEAM_ERR);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
				}

				//Inserts the team
				Team insertedTeam = _repository.InsertTeam(userEmail, team);
				
				//Maps the team to its DTO
				TeamDTO teamDTO = GenericMapper.MapToTeamDTO(insertedTeam);

				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Created, false,teamDTO, ConstantResponse.TEAM_CREATED);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.Created, response);

			}

		}

		[HttpPut]
		[Route("quit")]
		public HttpResponseMessage Quit()
		{
			//Gets the user from token
			var token = Request.Headers.Authorization.Parameter;
			string userEmail = UserUtility.GetEmailByToken(token);
			CustomResponse response;



			try
			{
				using (M32COMDBSERVER DB = new M32COMDBSERVER())
				{
					User loginUser = DB.Users.Include(x => x.receivedNotification).Include(x => x.team).Include(x => x.team.boat).First(x => x.email == userEmail);
					if (loginUser.team == null)
					{
						response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_QUIT_FAILED);
						return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);

					}
					Team team = DB.Teams.Include(x => x.teamMembers).Where(x => x.id == loginUser.teamId).First();

					//Team leader disposes the team
					if (team.leaderId == loginUser.id)
					{
						DB.Teams.Remove(team);
						DB.SaveChanges();
						response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, null, ConstantResponse.TEAM_DISPOSED);
						return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);

					}
					//Person quits from the team
					team.teamMembers.Remove(loginUser);
					DB.Entry(team).State = EntityState.Modified;
					DB.Entry(loginUser).State = EntityState.Modified;
					DB.SaveChanges();
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.OK, ConstantResponse.TEAM_QUIT_SUCCESS);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
				}
			}
			catch (DbEntityValidationException e)
			{
				foreach (var eve in e.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
				throw;
			}
			
		}

		[HttpPost]
		[Route("invite")]
		public HttpResponseMessage Invite([FromBody]UserEmailDTO emailDTO)
		{
			//Gets the Sender from token
			var token = Request.Headers.Authorization.Parameter;
			User sender = UserUtility.GetUserByToken(token);

			CustomResponse response;
			bool userHasTeam = _repository.UserHasTeam(sender.email);
			bool isUserLeader = _repository.IsUserLeader(sender.email);

			//Checks whether sender has a team or not
			if(!userHasTeam || !isUserLeader)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_SENDER_ERR);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}
			//Checks whether sender is trying to invite himself/herself
			else if(sender.email == emailDTO.email)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_YOURSELF);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}

			//used to get sender name etc..
			User senderLocal = _userRepository.GetByEmail(sender.email);
			User receiver = _userRepository.GetByEmail(emailDTO.email);

			//Checks whether the sender has already sent an invitation which is not answered by the receiver yet
			bool inProgressInvite = _notificationRepository.InProgressInvite(senderLocal.id, receiver.id);

			if (inProgressInvite)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAN_INVITE_INPROGRESS);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}
			//Checks whether the receiver is exist or not
			if (receiver == null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_RECEIVER_ERR);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}
			//Checks whether the receiver has already had a team or not
			else if(receiver.team != null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_RECEIVER_HAS_TEAM);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}

			//Helper class to create notification
			Notification notification = NotificationUtility.CreateForTeam(senderLocal,receiver.id);

			//Inserts created notification
			_notificationRepository.Insert(notification);
		
			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.OK, ConstantResponse.TEAM_INVITE_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK,response);
		}

		[HttpGet]
		[Route("respondinvitation/{accept:bool}")]
		public HttpResponseMessage RespondInvitation([FromUri] bool accept)
		{
			//Gets the headers
			var headers = Request.Headers;
			CustomResponse response;

			//if http packet does not include notification Token
			if (!headers.Contains("Actiontoken"))
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_RESPOND_MISSING_ACTIONTOKEN);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}

			//Notification token
			var actionToken = headers.GetValues("Actiontoken").First();


			//Gets the Receiver from Auth token
			var token = Request.Headers.Authorization.Parameter;

			//Gets the user who sent the request
			string loginUserEmail = UserUtility.GetEmailByToken(token);
			User loginUser = _userRepository.GetByEmail(loginUserEmail);


			//Gets the related notification from db by using unique actionToken
			Notification notification = _notificationRepository.getByToken(actionToken);

			//invalid notification check
			if (notification == null || !notification.isActive || notification.receivedBy.id != loginUser.id)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Forbidden, true, null, ConstantResponse.TEAM_RESPOND_INVALID_NOTIFICATION);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.Forbidden, response);
			}


			//Rejection check
			if (!accept)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.REJECTED, ConstantResponse.TEAM_RESPOND_REJECTED);
			}
			else
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.ACCEPTED, ConstantResponse.TEAM_RESPOND_ACCEPTED);
				//receiver is added to the team where sender is the leader
				_repository.AcceptTeamInvitation(loginUserEmail, notification.sentBy.teamId);
				//Makes notification passive
				
			}
			_notificationRepository.Update(notification, false);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK,response);

		}

		[HttpPost]
		[Route("addboat")]
		public HttpResponseMessage AddBoat([FromBody] Boat boat)
		{
			//Gets the user from token
			var token = Request.Headers.Authorization.Parameter;
			User sender = UserUtility.GetUserByToken(token);
			CustomResponse response;

			//Controls whether the user is leader or not
			if(sender.team == null || sender.team.leaderId != sender.id)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Unauthorized, true, HttpStatusCode.Unauthorized, ConstantResponse.TEAM_BOAT_FAILED);
				return Request.CreateResponse(HttpStatusCode.Unauthorized, response);
			}
			//Controls whether the users'team has already had a boat or not
			else if(sender.team.boat != null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, HttpStatusCode.BadRequest, ConstantResponse.TEAM_BOAT_ALREADY_HAVE_BOAT);
				return Request.CreateResponse(HttpStatusCode.BadRequest, response);
			}

			//Adds the boat to the team
			Boat addedBoat = _repository.AddBoat(sender.id, boat);
			//Maps the added boat to the its DTO
			BoatDTO boatDTO = GenericMapper.MapToBoatDTO(addedBoat);

			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, boatDTO, ConstantResponse.TEAM_BOAT_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK,response);

		}

		[HttpGet]
		[Route("get/boat")]
		public HttpResponseMessage GetBoat()
		{
			var token = Request.Headers.Authorization.Parameter;
			User sender = UserUtility.GetUserByToken(token);
			CustomResponse response;

			if(sender.team == null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, null, ConstantResponse.TEAM_BOAT_GET_NO_TEAM);
			}
			else if(sender.team.boat == null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, null, ConstantResponse.TEAM_BOAT_GET_NO_BOAT);
			}
			else
			{
				BoatDTO boatDTO = GenericMapper.MapToBoatDTO(sender.team.boat);

				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, boatDTO, ConstantResponse.TEAM_BOAT_GET_SUCCESS);
			}

			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}

		[HttpGet]
		[Route("members")]
		public HttpResponseMessage GetTeamMembers()
		{
			var token = Request.Headers.Authorization.Parameter;
			User user = UserUtility.GetUserByToken(token);
			Nullable<int> teamId = user.teamId;
			CustomResponse response;

			using(M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				if(teamId != null)
				{
					IEnumerable<UserDTO> result = _repository.GetTeamMembers(teamId);

					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, result, ConstantResponse.OK);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
				}
				//returns null if the user does not have a team
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, null, ConstantResponse.TEAM_GET_MEMBERS);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);

			}
		}

		[HttpGet]
		[SkipAuthFilterAttribute]
		[Route("getteamsbycompetitionid/{competitionId:int}")]
		public HttpResponseMessage GetTeamsByCompetitionId([FromUri] int competitionId)
		{
			CustomResponse response;

			ICollection<TeamCompetition> teams = _competitionRepository.GetTeamsByCompetitionId(competitionId);

			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, teams, ConstantResponse.OK);

			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
			
		}
	}
}