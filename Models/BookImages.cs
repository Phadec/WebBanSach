using Week2.Models;

namespace Week2.Models
{
    public class BookImages
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}