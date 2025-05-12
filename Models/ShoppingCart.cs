using System.ComponentModel.DataAnnotations;

namespace Week2.Models
{
    public class ShoppingCart
    {
        [Range(0, 10000)]
        public Book Book { get; set; }
        
        [Range(0, 10000)]
        public int Count { get; set; }
    }
}
