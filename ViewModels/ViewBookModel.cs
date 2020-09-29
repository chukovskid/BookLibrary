using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PDFUpload.Models
{
    public class ViewBookModel
    {
        public int Id { get; set; }
        public bool IsNew { get; set; } = true;
        public string Title { get; set; }
        public int Pages { get; set; }
        public string Author { get; set; }
        public byte[] ByteBook { get; set; }
        public IFormFile BookFile { get; set; }

        public IEnumerable<Tag> Tags { get; set; }



    }
}
