using System.Text.Json.Serialization;

namespace Week2.Models
{
    public class CartItem
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Cover { get; set; }
        
        [JsonIgnore]
        public Book Book { get; set; }
    }
}
