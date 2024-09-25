using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CarRent.Models
{
    public class Car
    {
        [Key]
        [Required]
        [StringLength(6)]
        public string PlateNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Fuel { get; set; }

        [Required]
        [StringLength(10)]
        public string Brand { get; set; }

        [Required]
        [Range(1000, 9999)]
        public int Year { get; set; }

        [Required]
        [Range(0, 9999)]
        public int Price { get; set; }

        [Required]
        public int CityId { get; set; }

        public string? UserId { get; set; }

        [StringLength(200)]
        public string? ImageFileName { get; set; }


        public string? ContentType { get; set; }

        public byte[]? Data { get; set; }

        [NotMapped]
        public virtual SiteUser? User { get; set; }

        [NotMapped]
        public virtual City? City { get; set; }
    }
}
