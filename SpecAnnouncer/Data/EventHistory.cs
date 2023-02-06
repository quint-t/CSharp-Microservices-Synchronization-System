using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecAnnouncer.Data
{
	public class EventHistory
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

		[Required]
		[Column(Order = 1)]
		public Event? Event { get; set; }

		[Required]
		[Column(Order = 2)]
		public DateTime DateTime { get; set; }

		[Required]
		[MaxLength(1024)]
		[Column(Order = 3)]
		public string? Data { get; set; }
	}
}
