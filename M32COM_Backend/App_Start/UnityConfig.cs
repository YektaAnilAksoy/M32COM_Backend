using M32COM_Backend.Repositories;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace M32COM_Backend
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>();
			container.RegisterType<IRegistrationRepository, RegistrationRepository>();
			container.RegisterType<ILoginRepository, LoginRepository>();
			container.RegisterType<IUserRepository, UserRepository>();
			container.RegisterType<ITeamRepository, TeamRepository>();
			container.RegisterType<INotificationRepository, NotificationRepository>();
			container.RegisterType<ICompetitionRepository, CompetitionRepository>();
			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}