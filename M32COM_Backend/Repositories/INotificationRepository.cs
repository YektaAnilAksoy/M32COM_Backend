using M32COM_Backend.DTOs;
using M32COM_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M32COM_Backend.Repositories
{
	public interface INotificationRepository
	{
		void Insert(Notification notification);
		bool InProgressInvite(int senderId, int receiverId);
		Notification getByToken(string token);
		void Update(Notification notification, bool passive);
		List<NotificationDTO> getNotifications(int userId);
	}
}
