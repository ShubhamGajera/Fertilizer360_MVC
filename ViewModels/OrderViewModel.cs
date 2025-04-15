// Add to Models folder: OrderViewModel.cs
namespace Fertilizer360.Models
{
    public class OrderViewModel
    {
        public int FertilizerId { get; set; }
        public string FertilizerName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}