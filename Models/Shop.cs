using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fertilizer360.Models
{
    [Table("shops")]
    public class Shop
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("image")]
        public string Image { get; set; } = "/uploads/default.png"; // Default image

        [Required]
        [Column("address")]
        public string Address { get; set; }

        [Required]
        [Column("phone_number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Column("work_time")]
        public string WorkTime { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("state")]
        public string State { get; set; }

        [Required]
        [Column("district")]
        public string District { get; set; }

        [Required]
        [Column("village_or_taluka")]
        public string VillageOrTaluka { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
