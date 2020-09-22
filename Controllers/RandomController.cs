using BookLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using PDFUpload.Models;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomController : ControllerBase
    {
        public readonly ApplicationDbContext _contex;
        public RandomController(ApplicationDbContext contex)
        {
            _contex = contex;
        }

        [HttpPost]
        public IActionResult Index()
        {

            var path = "D:/Employee_Report - Copy.pdf";
            Book book = new Book()
            {
                Author = "Dimitar C.",
                FilePathToBook = "D:/Employee_Report - Copy.pdf",
                ByteBook = System.IO.File.ReadAllBytes(path),
                Title = "Employee_Report - Copy"

            };

            _contex.Add(book);
            _contex.SaveChanges();




            return Content(book.Title);
        }



        [HttpPost("cfile")]
        public IActionResult CreatePdf()
        {
            var path = "D:/Employee_Report - Copy.pdf";
            Book book = new Book()
            {
                Author = "Dimitar C.",
                //FilePathToBook = "D:/Employee_Report - Copy.pdf",
                //ByteBook = System.IO.File.ReadAllBytes(path),
                //Title = "Employee_Report - Copy"

            };

            _contex.Add(book);
            _contex.SaveChanges();




            return Content(book.Title);
        }
    }
}