/*using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpecProfiles.Data
{
	public class P2PRelation
	{
		[Key]
		[Required]
		[MaxLength(32)]
		[Column(Order = 0)]
		public string? UniqueName { get; set; }

		[Required]
		[Column(Order = 1)]
		public Person? DeclarantPerson { get; set; }

		[Required]
		[Column(Order = 2)]
		public Person? RecipientPerson { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 3)]
		public string? DeclarantInfo { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 4)]
		public string? RecipientInfo { get; set; }

        [AllowNull]
        [MaxLength(1024)]
        [Column(Order = 5)]
        public string? AdditionalData { get; set; }
    }
}
*/