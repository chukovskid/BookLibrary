using BookLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PDFUpload.Dtos;
using PDFUpload.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILibraryRepository _repo;
        public SearchController(ILibraryRepository repo)
        {
            //_context = context;
            _repo = repo;
        }







        // Create      
        // POST     /api/Search/ProbaSearch
        [HttpPost]
        public async Task<IActionResult> Search(IEnumerable<ViewTagModel> bookTags)
        {


            var Books = await _repo.FoundBooks(bookTags);

            var dtoBooks = new List<BookListDto>();

            foreach (var book in Books)
            {
                var newBookDto = new BookListDto()
                {
                    Id = book.Id,
                    Pages = book.Pages,
                    Author = book.Author,
                    Title = book.Title,

                };
                dtoBooks.Add(newBookDto);
            }

            return Ok(dtoBooks);
        }




    }
}