/*using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpecProfiles.Data
{
	public class P2ORelation
	{
		[Key]
		[Required]
		[MaxLength(32)]
		[Column(Order = 0)]
		public string? UniqueName { get; set; }

		[Required]
		[Column(Order = 1)]
		public Person? Person { get; set; }

		[Required]
		[Column(Order = 2)]
		public Organization? Organization { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 3)]
		public string? PersonInfo { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 4)]
		public string? OrganizationInfo { get; set; }

		[AllowNull]
        [MaxLength(1024)]
        [Column(Order = 5)]
        public string? AdditionalData { get; set; }
    }
}
*/