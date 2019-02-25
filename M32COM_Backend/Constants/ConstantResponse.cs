using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.constants
{
	public static class ConstantResponse
	{
		public const string OK = "OK";
		public const string ACCEPTED = "ACCEPTED";
		public const string REJECTED = "REJECTED";
		public const string HAS_TEAM_ERR = "The user already has a team!";
		public const string TEAM_CREATED = "Team has successfully been created!";
		public const string TEAM_FAILED = "Given team name is already in use!";
		public const string UNAUTHORIZED_ACTION = "Unauthorized action!";
		public const string USER_CREATED = "User has successfully been created!";
		public const string EMAIL_IN_USE = "Given email is already in use!";
		public const string LOGIN_SUCCESS = "Login is successful!";
		public const string LOGIN_FAILED = "Email or password is wrong!";
		public const string TEAM_QUIT_FAILED = "The user does not have a team to quit!";
		public const string TEAM_QUIT_SUCCESS = "The user has quit from his team";
		public const string TEAM_DISPOSED = "Team lider has disposed the team!";
		public const string TEAM_INVITE_SUCCESS = "Team invitation has successfully been sent to the given user";
		public const string TEAM_INVITE_SENDER_ERR = "Notification sender does not have his own team";
		public const string TEAM_INVITE_YOURSELF = "You cannot invite yourself to join your team!";
		public const string TEAM_INVITE_RECEIVER_ERR = "There is no such receiver with the given email";
		public const string TEAM_INVITE_RECEIVER_HAS_TEAM = "Invited user already has a team!";
		public const string TEAM_RESPOND_MISSING_ACTIONTOKEN = "Missing action token in header!";
		public const string TEAM_RESPOND_INVALID_NOTIFICATION = "Invalid notification!";
		public const string TEAM_RESPOND_REJECTED = "The user has rejected the team invitation!";
		public const string TEAM_RESPOND_ACCEPTED = "The user has accepted the team invitation!";
		public const string TEAM_BOAT_SUCCESS = "The boat has successfully been added to the team!";
		public const string TEAM_BOAT_FAILED = "The user is not the team leader!";
		public const string TEAM_GET_MEMBERS = "The user does not have a team";
		public const string NOTIFICATION_ALL_SUCCESS = "User notifications have been retrieved!";
	}

}