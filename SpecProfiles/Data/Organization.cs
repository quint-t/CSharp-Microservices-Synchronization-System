using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpecProfiles.Data
{
	public class Organization
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

        [Required]
        [Column(Order = 1)]
        public string? OwnerId { get; set; }

        [Required]
		[MaxLength(128)]
		[Column(Order = 2)]
		public string? Email { get; set; }

		[Required]
		[MaxLength(16)]
		[Column(Order = 3)]
		public string? PhoneNumber { get; set; }
        
		[Required]
        [Column(Order = 4, TypeName = "Date")]
        public DateTime EstablishmentDate { get; set; }

        [Required]
		[MaxLength(128)]
		[Column(Order = 5)]
		public string? Type { get; set; } // "corporation", "non-profit", "government agency"

		[Required]
		[MaxLength(128)]
		[Column(Order = 6)]
		public string? Industry { get; set; } // "agriculture", "construction", "retail trade"

		[Required]
		[MaxLength(64)]
		[Column(Order = 7)]
		public string? Country { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 8)]
		public string? City { get; set; }

		[Required]
		[MaxLength(140)]
        [Column(Order = 9, TypeName = "text")]
		public string? ShortInfo { get; set; }

        [AllowNull]
        [MaxLength(1024)]
        [Column(Order = 10)]
        public string? AdditionalData { get; set; }
    }
}
