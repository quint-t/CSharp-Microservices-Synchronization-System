using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpecProfiles.Data
{
	public class Person
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

        [Required]
        [Column(Order = 1)]
        public int OwnerId { get; set; }

        [Required]
		[MaxLength(64)]
		[Column(Order = 2)]
		public string? FirstName { get; set; }

		[Required]
		[MaxLength(64)]
		[Column(Order = 3)]
		public string? LastName { get; set; }

		[Required]
        [Column(Order = 4, TypeName = "Date")]
		public DateTime BirthDate { get; set; }

		[Required]
		[MaxLength(16)]
		[Column(Order = 5)]
		public string? Gender { get; set; }

		[Required]
		[MaxLength(64)]
		[Column(Order = 6)]
		public string? Country { get; set; }

		[Required]
		[MaxLength(128)]
		[Column(Order = 7)]
		public string? City { get; set; }

		[Required]
		[MaxLength(140)]
		[Column(Order = 8, TypeName = "text")]
        public string? ShortInfo { get; set; }

		[Required]
		[MaxLength(22)]
		[Column(Order = 9)]
		public string? FamilyStatus { get; set; }

        [AllowNull]
        [MaxLength(1024)]
        [Column(Order = 10)]
        public string? AdditionalData { get; set; }
    }
}
