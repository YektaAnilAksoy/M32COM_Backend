using M32COM_Backend.constants;
using M32COM_Backend.Filter;
using M32COM_Backend.Models;
using M32COM_Backend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace M32COM_Backend.Controllers
{
	[ErrorAttribute]
	[ActionAttribute]
	[ModelValidationAttribute]
	[AuthorizationAttribute]
	[RoutePrefix("api/competition")]
	public class CompetitionController : ApiController
    {
		private M32COMDBSERVER DB = new M32COMDBSERVER();

		[HttpGet]
		[Route("get/active")]
		public HttpResponseMessage GetActiveCompetitions()
		{
			var activeCompetitions = DB.Competitions.Where(x => x.endDate > DateTime.Now).OrderByDescending(x => x.startDate).ToList();
			CustomResponse response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, activeCompetitions, ConstantResponse.COMPETITION_RETRIEVE_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}

		[HttpGet]
		[Route("get/{competitionId}")]
		public HttpResponseMessage Get(int competitionId)
		{
			var result = DB.Competitions.Where(x => x.id == competitionId).FirstOrDefault();
			CustomResponse response;
			if (result == null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.NoContent, false, result, ConstantResponse.COMPETITION_RETRIEVE_BY_ID_NOT_FOUND);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.NoContent, response);
			}
			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, result, ConstantResponse.COMPETITION_RETRIEVE_BY_ID_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}


		//implement edilecek 
		[HttpPost]
		[Route("{competitionId}/apply")]
		public HttpResponseMessage Apply(int competitionId)
		{
			var token = Request.Headers.Authorization.Parameter;
			string userEmail = UserUtility.GetEmailByToken(token);
			User user = DB.Users.Include(x=>x.team).Where(x => x.email == userEmail).FirstOrDefault();
			CustomResponse response;
			//checks whether user is the team leader
			if (user.team != null && user.team.leaderId == user.id)
			{
				Competition comp = DB.Competitions.Where(x => x.id == competitionId).FirstOrDefault();
				if(comp != null)
				{
					Team userTeam = user.team;
					TeamCompetition teamComp = new TeamCompetition();
					teamComp.competition = comp;
					teamComp.competitionId = comp.id;
					teamComp.team = userTeam;
					teamComp.teamId = userTeam.id;
					teamComp.finishTime = CompetitionUtility.GetRandomDay(comp.startDate, comp.endDate);
					DB.TeamCompetitions.Add(teamComp);
					DB.SaveChanges();

					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, HttpStatusCode.OK, ConstantResponse.COMPETITION_APPLIED_SUCCESS);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
				}
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.NotFound, true, HttpStatusCode.NotFound, ConstantResponse.COMPETITITON_APPLIED_NO_COMP_FOUND);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.NotFound, response);
			}
			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, HttpStatusCode.BadRequest, ConstantResponse.COMPETITION_APPLIED_NOT_TEAMLEADER);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
		}

		// Will be completed
		[HttpGet]
		[Route("{competitionId}/teams")]
		public HttpResponseMessage GetTeamsByCompetitionId(int competitionId)
		{

			//return teamDTO
			return null;
		}
    }
}
