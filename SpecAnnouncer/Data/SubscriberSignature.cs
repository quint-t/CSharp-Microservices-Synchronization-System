using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecAnnouncer.Data
{
	public class SubscriberSignature
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

		[Column(Order = 1)]
		[Required]
		public Subscriber? Subscriber { get; set; }

		[Column(Order = 2)]
		[Required]
		public Event? Event { get; set; }

		[Column(Order = 3)]
		[Required]
		public int Priority { get; set; }
	}
}
