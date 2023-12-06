using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebProject.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int TableNo { get; set; }
        public string? CustomerName { get; set; }
        [NotMapped]
        public List<string>? Items { get; set; }
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