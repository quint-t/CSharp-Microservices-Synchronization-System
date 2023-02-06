using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecAnnouncer.Data
{
	public class Event
	{
		[Key]
		[Required]
		[MaxLength(32)]
		[Column(Order = 0)]
		public string? UniqueName { get; set; }
	}
}
