using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Models
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string TagName { get; set; }
        public bool ForNewBook { get; set; }
        public ICollection<BookTags> BookTags { get; set; }
    }
}
