using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeApp.Models;


namespace CafeApp.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int? TableNo { get; set; }
        public string? CustomerName { get; set; }
        
        public List<SelectedProduct>? Products { get; set; }
        public float Price {get; set;}
        public bool IsServed { get; set; }
        public bool IsPaid { get; set; }

        public Order()
        {
            IsServed = false;
            IsPaid = false;
        }
    }
}