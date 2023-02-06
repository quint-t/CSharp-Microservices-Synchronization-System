using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecAnnouncer.Data
{
	public class Subscriber
	{
		[Key]
		[Required]
		[MaxLength(32)]
		[Column(Order = 0)]
		public string? UniqueName { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 1)]
		public string? Url { get; set; }
	}
}
