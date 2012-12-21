using System.Data.Entity;
using Tax.Model;

namespace Tax.Persistence
{
	public class Context : DbContext
	{
		public DbSet<Company> Companies { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
		}
	}
}
