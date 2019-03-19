﻿using M32COM_Backend.constants;
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
using M32COM_Backend.Mappers;
using M32COM_Backend.DTOs;

namespace M32COM_Backend.Controllers
{
	[ErrorAttribute]
	[ActionAttribute]
	[ModelValidationAttribute]
	
	[RoutePrefix("api/competition")]
	public class CompetitionController : ApiController
	{
		private M32COMDBSERVER DB = new M32COMDBSERVER();


		[HttpGet]
		[Route("get/all")]
		public HttpResponseMessage GetAllCompetitions()
		{
			//Retrieves all comps from db
			var allComps = DB.Competitions.ToList();

			//Null check
			if (allComps == null)
				allComps = new List<Competition>();

			//Maps comps to the dto
			ICollection<CompetitionDTO> competitionDTOs = new List<CompetitionDTO>();
			allComps.ForEach(c => competitionDTOs.Add(GenericMapper.MapToCompetitionDTO(c)));

			//Returns data
			CustomResponse response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, competitionDTOs, ConstantResponse.COMPETITION_ALL_RETRIEVE_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);

		}

		[HttpGet]
		[Route("get/active")]
		public HttpResponseMessage GetActiveCompetitions()
		{
			//Retrieves active comps from db
			var activeCompetitions = DB.Competitions.Where(x => x.endDate > DateTime.Now).OrderByDescending(x => x.startDate).ToList();

			//Null check
			if (activeCompetitions == null)
				activeCompetitions = new List<Competition>();
			
			//Maps active comps to the dto
			ICollection<CompetitionDTO> competitionDTOs = new List<CompetitionDTO>();
			activeCompetitions.ForEach(c => competitionDTOs.Add(GenericMapper.MapToCompetitionDTO(c)));

			//Returns data
			CustomResponse response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, competitionDTOs, ConstantResponse.COMPETITION_RETRIEVE_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}

		[HttpGet]
		[Route("get/passive/{rowCount}")]
		public HttpResponseMessage GetLastPassiveCompetitionsByRowCount(int rowCount)
		{
			//Retrieves Last  {rowCount} competitions which are currently not active.
			var competitions = DB.Competitions.Where(x => x.endDate < DateTime.Now).OrderByDescending(d => d.startDate).Take(rowCount).ToList();

			CustomResponse response;
			if (competitions == null)
				competitions = new List<Competition>();

			ICollection<CompetitionDTO> competitionDTOs = new List<CompetitionDTO>();
			competitions.ForEach(c => competitionDTOs.Add(GenericMapper.MapToCompetitionDTO(c)));

			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, competitionDTOs, ConstantResponse.COMPETITION_PASSIVES_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}

		[HttpGet]
		[Route("get/{competitionId}")]
		public HttpResponseMessage Get(int competitionId)
		{
			//Gets the competition related with the given id
			var competition = DB.Competitions.Where(x => x.id == competitionId).FirstOrDefault();
			CustomResponse response;

			//Null check
			if (competition == null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.NotFound, true, HttpStatusCode.NotFound, ConstantResponse.COMPETITION_RETRIEVE_BY_ID_NOT_FOUND);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.NotFound, response);
			}

			//Mapping to DTO
			var competitionDTO = GenericMapper.MapToCompetitionDTO(competition);

			//Returns data
			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, competitionDTO, ConstantResponse.COMPETITION_RETRIEVE_BY_ID_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}



		[AuthorizationAttribute]
		[HttpPost]
		[Route("{competitionId}/apply")]
		public HttpResponseMessage Apply(int competitionId)
		{
			var token = Request.Headers.Authorization.Parameter;
			string userEmail = UserUtility.GetEmailByToken(token);
			
			User user = DB.Users.Include(x => x.team).Where(x => x.email == userEmail).FirstOrDefault();
			
			CustomResponse response;

			//checks whether user is the team leader
			if (user.team != null && user.team.leaderId == user.id)
			{
				Competition comp = DB.Competitions.Where(x => x.id == competitionId).FirstOrDefault();

				//Null check for competition
				if (comp == null)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.NotFound, true, HttpStatusCode.NotFound, ConstantResponse.COMPETITITON_APPLIED_NO_COMP_FOUND);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.NotFound, response);
				}
				//Checks whether competition is active or not
				else if(comp.endDate < DateTime.Now)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, HttpStatusCode.BadRequest, ConstantResponse.COMPETITION_APPLIED_NOT_ACTIVE);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
				}

				//Checks whether the team has already applied or not
				var teamAlreadyApplied = DB.TeamCompetitions.Where(x => x.competitionId == competitionId && x.teamId == user.teamId).FirstOrDefault();
				if(teamAlreadyApplied != null)
				{
					response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, HttpStatusCode.BadRequest, ConstantResponse.COMPETITION_APPLIED_ALREADY);
					return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
				}

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
			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.BadRequest, true, HttpStatusCode.BadRequest, ConstantResponse.COMPETITION_APPLIED_NOT_TEAMLEADER);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.BadRequest, response);
		}





		
		[HttpGet]
		[Route("{competitionId}/result")]
		public HttpResponseMessage GetResultByCompetitionId(int competitionId)
		{
			//Retrieves the result of the competition related with the given id
			var teamCompetitions = DB.TeamCompetitions.Where(x => x.competitionId == competitionId).OrderBy(d => d.finishTime).ToList();

			CustomResponse response;

			if (teamCompetitions == null)
			{
				response = ResponseMessageHelper.CreateResponse(HttpStatusCode.NotFound, true, HttpStatusCode.NotFound, ConstantResponse.COMPETITION_RESULT_NOT_FOUND);
				return Request.CreateResponse<CustomResponse>(HttpStatusCode.NotFound, response);
			}

			//Mapping to related DTO
			CompetitionResultDTO result = GenericMapper.MapToCompetitionResultDTO(teamCompetitions);

			response = ResponseMessageHelper.CreateResponse(HttpStatusCode.OK, false, result, ConstantResponse.COMPETITION_RESULT_SUCCESS);
			return Request.CreateResponse<CustomResponse>(HttpStatusCode.OK, response);
		}

    }
}
