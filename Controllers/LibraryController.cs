using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using PDFUpload.Models;
using PDFUpload.Repos;
using System.IO;
using System.Threading.Tasks;


namespace BookLibrary.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryRepository _repo;
        public LibraryController(ILibraryRepository repo)
        {
            _repo = repo;
        }



        public async Task<IActionResult> Index()
        {

            return View();
        }

        public async Task<IActionResult> Search()
        {

            return View();
        }
        public async Task<IActionResult> PdfCanva()
        {

            return View();
        }



        public async Task<IActionResult> New()
        {
            var repoTags = await _repo.GetTags(null);
            var viewModel = new ViewBookModel
            {
                Tags = repoTags
            };
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Create(ViewBookModel viewBook)
        {

            var TagsForNewBoook = await _repo.TagsForNewBook();

            using (var ms = new MemoryStream())
            {
                viewBook.BookFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                viewBook.ByteBook = fileBytes;
            }

            var newBook = new Book()
            {
                Id = GetHashCode(),
                Author = viewBook.Author,
                Title = viewBook.Title,
                ByteBook = viewBook.ByteBook,
                IsNew = viewBook.IsNew
            };

            foreach (var tag in TagsForNewBoook)
            {
                tag.ForNewBook = false;
                var bookTag = new BookTags()
                {
                    BookId = newBook.Id,
                    TagId = tag.Id
                };
                _repo.Add(bookTag);
            }
            _repo.Add(newBook);

            if (await _repo.SaveAll())
            {
                var url = "https://localhost:44307/api/books/";
                return Redirect(url + newBook.Id);
            }

            return Ok("Was not saved");


        }









    }
}