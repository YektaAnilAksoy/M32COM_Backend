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
using System.Data.Entity.Infrastructure;
using M32COM_Backend.constants;
using M32COM_Backend.DTOs;
using M32COM_Backend.Mappers;

namespace M32COM_Backend.Controllers
{
	[ErrorAttribute]
	[ActionAttribute]
	[ModelValidationAttribute]
	[AuthorizationAttribute]
	[RoutePrefix("api/team")]
	public class TeamController : ApiController
	{
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
				if (!TeamUtility.IsTeamNameUnique(team.name))
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_FAILED);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);

				}
				User loginUser =  DB.Users.Include(x => x.receivedNotification).Include(x => x.team).Include(x => x.team.boat).First(x => x.email == userEmail);
				if (loginUser.team != null)
				{

					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest,true,null, ConstantResponse.HAS_TEAM_ERR);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
				}
				team.leaderId = loginUser.id;
				team.teamMembers.Add(loginUser);
				DB.Teams.Add(team);
				DB.SaveChanges();

				TeamDTO teamDTO = GenericMapper.MapToTeamDTO(team);
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
			//Checks whether sender has a team or not
			if(sender.team == null || sender.team.leaderId != sender.id)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_SENDER_ERR);

				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}else if(sender.email == emailDTO.email)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_YOURSELF);

				return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
			}

			//Adds the notification to  the receiver's notification list
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				User receiver = DB.Users.Where(x => x.email == emailDTO.email).FirstOrDefault();
				User senderLocal = DB.Users.Where(x => x.email == sender.email).FirstOrDefault();
				if(receiver == null)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_RECEIVER_ERR);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
				}else if(receiver.team != null)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, null, ConstantResponse.TEAM_INVITE_RECEIVER_HAS_TEAM);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
				}

				Notification notification = NotificationUtility.CreateForTeam(senderLocal);
				notification.receivedBy = receiver;
				notification.sentBy = senderLocal;
				DB.Notifications.Add(notification);
				DB.SaveChanges();
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.OK, ConstantResponse.TEAM_INVITE_SUCCESS);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK,response);
			}


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

				return Request.CreateResponse(HttpStatusCode.BadRequest, response);

			}

			//Notification token
			var actionToken = headers.GetValues("Actiontoken").First();

			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				//Gets the Receiver from Auth token
				var token = Request.Headers.Authorization.Parameter;
				//Gets the user who sent the request
				string loginUserEmail = UserUtility.GetEmailByToken(token);
				User loginUser = DB.Users.Include(x => x.receivedNotification).Include(x => x.team).Include(x => x.team.boat).First(x => x.email == loginUserEmail);

				//Gets the related notification from db by using unique actionToken
				Notification notification = DB.Notifications.Where(x => x.actionToken == actionToken).FirstOrDefault();

				//invalid notification check
				if (notification == null || !notification.isActive || notification.receivedBy.id != loginUser.id)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Forbidden, true, null, ConstantResponse.TEAM_RESPOND_INVALID_NOTIFICATION);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.Forbidden, response);
				}

				notification.isActive = false;

				//Rejection check
				if (!accept)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.REJECTED, ConstantResponse.TEAM_RESPOND_REJECTED);
				}
				else
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, ConstantResponse.ACCEPTED, ConstantResponse.TEAM_RESPOND_ACCEPTED);
					//receiver is added to the team where sender is the leader
					loginUser.teamId = DB.Users.Include(b => b.team).Include(x => x.team.boat).Where(x => x.id == notification.sentBy.id).Select(x => x.teamId).SingleOrDefault();
				}			
				DB.SaveChanges();
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK,response);
			}
		}

		[HttpPost]
		[Route("addboat")]
		public HttpResponseMessage AddBoat([FromBody] Boat boat)
		{
			//Gets the user from token
			var token = Request.Headers.Authorization.Parameter;
			User sender = UserUtility.GetUserByToken(token);
			CustomResponse response;
			//Control whether the user is leader or not
			if(sender.team == null || sender.team.leaderId != sender.id)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.Unauthorized, true, null, ConstantResponse.TEAM_BOAT_FAILED);
				return Request.CreateResponse(HttpStatusCode.Unauthorized, response);
			}

			//Adds the boat to the team
			using (M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				boat.id = (int)sender.teamId;
				boat.team = sender.team;
				DB.Boats.Add(boat);
				DB.SaveChanges();
				BoatDTO boatDTO = GenericMapper.MapToBoatDTO(boat);

				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, boatDTO, ConstantResponse.TEAM_BOAT_SUCCESS);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK,response);
			}
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
					IEnumerable<UserDTO> result = DB.Users.Include(x => x.team).Where(x => x.team.id == teamId).Select(x => new UserDTO { id = x.id, name = x.name, surname = x.surname, email = x.email,teamName = x.team.name }).ToList();
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, result, ConstantResponse.OK);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
				}
				//returns null if no team
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
			using(M32COMDBSERVER DB = new M32COMDBSERVER())
			{
				Competition competition = DB.Competitions.Where(x => x.id == competitionId).FirstOrDefault();
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, competition?.teams, ConstantResponse.OK);

				return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
			}
		}
	}
}