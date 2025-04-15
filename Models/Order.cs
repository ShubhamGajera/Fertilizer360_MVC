using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fertilizer360.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("FertilizerId")]
        public int FertilizerId { get; set; }

        [ForeignKey("FertilizerId")]
        public virtual Fertilizer Fertilizer { get; set; }

        [Required]
        [Column("ShopId")]
        public int ShopId { get; set; }

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }

        [Required]
        [Column("Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("Price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column("Subtotal", TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [Required]
        [Column("OrderDate")]
        public DateTime OrderDate { get; set; }

        [Column("UserName")]
        [MaxLength(100)]
        public string? UserName { get; set; }
    }
}
