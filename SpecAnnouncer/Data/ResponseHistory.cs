using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecAnnouncer.Data
{
	public class ResponseHistory
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

		[Required]
		[Column(Order = 1)]
		public Subscriber? Subscriber { get; set; }

		[Required]
		[Column(Order = 2)]
		public EventHistory? EventHistory { get; set; }

		[Required]
		[Column(Order = 3)]
		public bool ResponseReceived { get; set; }
	}
}
