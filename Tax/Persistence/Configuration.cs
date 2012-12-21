using System.Data.Entity.Migrations;

namespace Tax.Persistence
{
	public class Configuration : DbMigrationsConfiguration<Context>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}
	}
}
