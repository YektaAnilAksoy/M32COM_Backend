namespace M32COM_Backend
{
	using M32COM_Backend.Models;
	using System;
	using System.Data.Entity;
	using System.Linq;

	public class M32COMDBSERVER : DbContext
	{
		// Your context has been configured to use a 'M32COMDBSERVER' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'M32COM_Backend.M32COMDBSERVER' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'M32COMDBSERVER' 
		// connection string in the application configuration file.
		public M32COMDBSERVER()
			: base("name=M32COMDBSERVER")
		{
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		// public virtual DbSet<MyEntity> MyEntities { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Team> Teams { get; set; }
		public virtual DbSet<Log> Logs { get; set; }
	}

	//public class MyEntity
	//{
	//    public int Id { get; set; }
	//    public string Name { get; set; }
	//}
}