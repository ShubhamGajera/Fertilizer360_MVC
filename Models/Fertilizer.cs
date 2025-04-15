using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fertilizer360.Models
{
    [Table("fertilizer")]
    public class Fertilizer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("shops_id")]
        public int ShopId { get; set; }

        [ForeignKey("ShopId")]
        public virtual Shop? Shop { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required]
        public int Stocks { get; set; }

        public string? Image { get; set; }
    }
}
