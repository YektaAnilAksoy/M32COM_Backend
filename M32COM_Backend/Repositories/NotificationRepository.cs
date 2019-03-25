using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using M32COM_Backend.DTOs;
using M32COM_Backend.Models;

namespace M32COM_Backend.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private M32COMDBSERVER DB = new M32COMDBSERVER();

		//Returns notification by its ActionToken
		public Notification getByToken(string token)
		{
			return DB.Notifications.Where(x => x.actionToken == token).FirstOrDefault();
		}

		//Returns all notifications of the user
		public List<NotificationDTO> getNotifications(int userId)
		{
			return DB.Notifications.Where(x => x.isActive == true && x.receivedById == userId).Select(x => new NotificationDTO { id = x.id, receivedById = x.receivedById, receivedByNameSurname = x.receivedBy.name + " " + x.receivedBy.surname, sentById = x.sentById, sentByNameSurname = x.sentBy.name + " " + x.sentBy.surname, actionToken = x.actionToken, description = x.description, sentTime = x.sentTime }).ToList();
		}

		//Checks whether the sender has already sent an invitation to the receiver
		public bool InProgressInvite(int senderId, int receiverId)
		{
			return DB.Notifications.Any(x => x.sentById == senderId && x.receivedById == receiverId && x.isActive == true);
		}

		//Inserts the notificiation to the DB
		public void Insert(Notification notification)
		{
			DB.Notifications.Add(notification);
			DB.SaveChanges();
		}

		//Makes the notification passive
		public void Update(Notification notification,bool passive)
		{

			notification.isActive = passive;
			DB.Notifications.Attach(notification);
			DB.Entry(notification).Property(x => x.isActive).IsModified = true;
			DB.SaveChanges();
		}
	}
}