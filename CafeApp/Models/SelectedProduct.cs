using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CafeApp.Models
{
    public class SelectedProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SelectedProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; } 
        

    }
}