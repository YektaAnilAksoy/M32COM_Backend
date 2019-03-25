using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M32COM_Backend.Utility
{
	public static class NotificationUtility
	{
		//Creates a notification for team invitation
		public static Notification CreateForTeam(User sender,int receiverId)
		{
			Notification notification = new Notification();
			notification.sentById = sender.id;
			notification.receivedById = receiverId;
			notification.sentTime = DateTime.Now;
			notification.isActive = true;
			notification.description = sender.name + " " + sender.surname + " has invited you to join " + sender.team.name;
			notification.actionToken = CreateGuid();

			return notification;
		}


		public static string CreateGuid()
		{
			return Guid.NewGuid().ToString();
		}
	}
}