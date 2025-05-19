using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Week2.Models;

namespace Week2.Models
{
    public class ShoppingCart
    {
        [Range(0, 10000)]
        public Book Book { get; set; }
        
        [Range(0, 10000)]
        public int Count { get; set; }
        
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.BookId == item.BookId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        
        public void RemoveItem(int bookId)
        {
            Items.RemoveAll(i => i.BookId == bookId);
        }
    }
}
