using Microsoft.EntityFrameworkCore;

namespace SpecAnnouncer.Data
{
    public class ApplicationDbContext : DbContext
    {
		public DbSet<Event> Events { get; set; }
        public DbSet<EventHistory> EventsHistory { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SubscriberSignature> SubscribersSignatures { get; set; }
        public DbSet<ResponseHistory> ResponsesHistory { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора.
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора.

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
