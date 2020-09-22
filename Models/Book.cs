using BookLibrary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDFUpload.Models
{
    public class Book
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public string Author { get; set; }
        public string HTMLBook { get; set; }
        public byte[] ByteBook { get; set; }
        public string FilePathToBook { get; set; }
        public string TagId { get; set; }
        public bool IsNew { get; set; }

        public ICollection<BookTags> BookTags { get; set; } // ovaa kniga i Tagot ke gi imaat istiot Id na BookTag



        //public IFormFile BookFile { get; set; }
        //public MemoryStream BookDocument { get; set; }


    }
}
