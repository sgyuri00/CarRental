using System.ComponentModel.DataAnnotations;

namespace CarRent.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        [Required]
        [StringLength(4)]
        public int ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string County { get; set; }

    }
}