using PDFUpload.Models;

namespace BookLibrary.Models
{
    public class BookTags
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Tag Tag { get; set; }
        public int BookId { get; set; }
        public int TagId { get; set; }
    }
}
