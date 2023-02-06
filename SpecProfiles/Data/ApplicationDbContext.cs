using Microsoft.EntityFrameworkCore;
using SpecProfiles.Data;

namespace SpecProfiles.Data
{
    public class ApplicationDbContext : DbContext
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<SpecProfiles.Data.Person> Person { get; set; } = default!;

		public DbSet<SpecProfiles.Data.Organization> Organization { get; set; } = default!;

	}
}
