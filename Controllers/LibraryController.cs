using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
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


        public IFormFile nesto { get; set; }




        public async Task<IActionResult> Index()
        {

            //var Books = _repo.GetBooks();



            return View(); //Books
        }

        public async Task<IActionResult> Search()
        {

            //var Books = _repo.GetBooks();



            return View(); //Books
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


            //_repo.SaveAll();

            //var url = "https://localhost:44307/api/books/";
            //return Redirect(url + newBook.Id);
            if (await _repo.SaveAll())
            {

                var url = "https://localhost:44307/api/books/";
                return Redirect(url + newBook.Id);


            }

            return Ok("Was not saved");

            //var path = "D:/Employee_Report - Copy.pdf";
            //Book viewBook = new Book()
            //{
            //    Author = "Dimitar C.",
            //    FilePathToBook = "D:/Employee_Report - Copy.pdf",
            //    ByteBook = System.IO.File.ReadAllBytes(path),
            //    Title = "Employee_Report - Copy"

            //};

            // From File to Byte[]

            // return new EmptyResult();
            //return Ok();
            // return new HttpStatusCodeResult(204);

        }




        public async Task<IActionResult> New()
        {
            var repoTags = await _repo.GetTags(null);
            var viewModel = new ViewBookModel
            {
                Tags = repoTags
            };
            //viewModel.Tags = new SelectList(repoTags, "Id", "TagName", 1);
            return View(viewModel);
        }




        //public ActionResult Create(Book book)
        //{


        //    return View("CustomerForm", Book);
        //}



    }
}