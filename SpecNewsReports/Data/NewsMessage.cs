using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpecNewsReports.Data
{
	public class NewsMessage
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

		[Required]
		[Column(Order = 1)]
		public string? OwnerId { get; set; }

		[AllowNull]
		[Column(Order = 2)]
		public string? PersonId { get; set; }

		[AllowNull]
		[Column(Order = 3)]
        public string? OrganizationId { get; set; }

        [Required]
		[MaxLength(512)]
		[Column(Order = 4)]
        public string? Caption { get; set; }

        [Required]
		[MaxLength(65536)]
		[Column(Order = 5)]
        public string? Content { get; set; }

		[Required]
		[Column(Order = 6)]
		public DateTime CreationDate { get; set; }

		[Required]
		[Column(Order = 7)]
		public DateTime LastChangeDate { get; set; }

		[AllowNull]
        [MaxLength(1024)]
        [Column(Order = 8)]
        public string? AdditionalData { get; set; }
    }
}
