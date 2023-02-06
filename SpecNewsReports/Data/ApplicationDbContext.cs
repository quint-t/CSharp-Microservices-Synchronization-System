using Microsoft.EntityFrameworkCore;
using SpecNewsReports.Data;

namespace SpecNewsReports.Data
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

		public DbSet<SpecNewsReports.Data.NewsMessage> NewsMessage { get; set; } = default!;

	}
}
