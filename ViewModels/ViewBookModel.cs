using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace PDFUpload.Models
{
    public class ViewBookModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsNew { get; set; } = true;
        public string Title { get; set; }
        public int Pages { get; set; }
        public string Author { get; set; }
        public string HTMLBook { get; set; }
        public byte[] ByteBook { get; set; }
        public string FilePathToBook { get; set; }
        public IFormFile BookFile { get; set; }

        public ICollection<BookTags> BookTags { get; set; }

        public IEnumerable<Tag> TagModels { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<int> TagIds { get; set; }
        public string[] TagStringArr { get; set; }
        public List<string> TagStringIds { get; set; }
        public int SelectedTag { get; set; }
        public List<string> SelectedValues { get; set; }



        //public MemoryStream BookDocument { get; set; }


    }
}
