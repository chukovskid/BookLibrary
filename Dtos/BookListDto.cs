using System.Collections.Generic;

namespace PDFUpload.Dtos
{
    public class BookListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public string Author { get; set; }
        public List<int> TagIds{ get; set; }

    }
}
