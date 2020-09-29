using BookLibrary.Models;
using BookLibrary.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using PDFUpload.Dtos;
using PDFUpload.Models;
using PDFUpload.Repos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFUpload.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class BooksController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly ILibraryRepository _repo;
        public BooksController(/*ApplicationDbContext context,*/ ILibraryRepository repo)
        {
            //_context = context;
            _repo = repo;
        }




        // Create      
        // POST     /api/Books/createBook
        [HttpPost("createBook")]
        public async Task<IActionResult> CreateBook(ViewBookModel viewBook)
        {
            using (var ms = new MemoryStream())
            {
                viewBook.BookFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                viewBook.ByteBook = fileBytes;
            }

            var book = new Book()
            {
                Author = viewBook.Author,
                Title = viewBook.Title,
                ByteBook = viewBook.ByteBook
            };


            var repoTag = await _repo.GetTag(3);
            //var newTag = new Tag() { TagName = "NovTag" };

            var bookTag = new BookTags()
            {
                TagId = repoTag.Id,
                BookId = book.Id

            };
            _repo.Add(bookTag);


            _repo.Add(book);
            if (await _repo.SaveAll())
            {
                var url = "https://localhost:44307/api/books/";
                return Redirect(url + book.Id);

            }

            return Ok("Was not saved");
        }


        // List Dto
        // GET     /api/books
        [HttpGet]
        public async Task<IActionResult> Index() // EngineerVM engineerVM
        {
            var allBooks = await _repo.GetBooks();
            //var allBooks = await _repo.GetBooks();

            // tuka treba AutoMapper
            var allBooksDto = new List<BookListDto>();
            foreach (var book in allBooks)
            {
                BookListDto BookListDto = new BookListDto()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Title = book.Title,
                    Pages = book.Pages

                };
                allBooksDto.Add(BookListDto);
            }
            // do tuka so Mapp


            return Ok(allBooksDto);
        }






        [HttpPost("newBookTags")]
        public async Task<IActionResult> CreateBookTag(List<ViewTagModel> bookTags)
        {
            //var newBook = _repo.GetNewBook();

            List<Tag> Tags = new List<Tag>();
            Random rnd = new Random();
            List<ViewTagModel> noDuplicateBookTags = bookTags.Distinct().ToList();

            foreach (var tag in noDuplicateBookTags) // create the tag it its New (id != int) bidejki front ti vrakja id=tagName
            {
                int id;
                if (!(int.TryParse(tag.id, out id)))
                {
                    var newTag = new Tag()
                    {
                        Id = rnd.Next(1, 8000) + GetHashCode(),
                        TagName = tag.tagName,
                        ForNewBook = true
                    };

                    // vidi dali ke proveruvas dali postoi vekje ovoj tag pred da go kreiras,
                    // Posledica e prepolna baza so isto imenuvani Tagovi (mozda i go sakas toa)
                    _repo.Add(newTag); // add New // moze i so AddTage(Tags)
                    Tags.Add(newTag);
                }
                else
                {
                    int tagId;
                    int.TryParse(tag.id, out tagId);

                    var repoTag = await _repo.GetTag(tagId);
                    repoTag.ForNewBook = true;

                    Tags.Add(repoTag);
                }

            }

            //foreach (var tag in Tags)
            //{
            //    tag.ForNewBook = true;
            //    //var bookTag = new BookTags()
            //    //{
            //    //    BookId = newBook.Id,
            //    //    TagId = tag.Id
            //    //};
            //    //_repo.Add(bookTag);
            //}


            //_repo.SaveAll();

            if (await _repo.SaveAll())
            {
                return Ok();

            }

            return Ok("Done");

            //var url = "https://localhost:44307/api/books/";
            //return Redirect(url + newBook.Id);

            //return Ok(bookTags);
        }



        // Post     /api/books/{id}/tag/{tagId}
        [HttpPost("{bookId}/Tag/{tagId}")]
        public async Task<IActionResult> CreateBookTag(int bookId, int tagId) // ova ke bide za sekoj nov tag koga ke sakam da go stavam
        {
            var bookTag = new BookTags()
            {
                TagId = tagId,
                BookId = bookId
            };
            _repo.Add(bookTag);
            await _repo.SaveAll();

            return Ok("Tag,Saved");
        }





        //api/books/newTag
        // Post
        [HttpPost("newTag")]
        public async Task<IActionResult> CreateTag(string tagName) //string filePath
        {
            var newTag = new Tag()
            {
                TagName = tagName
            };


            _repo.Add(newTag);
            await _repo.SaveAll();

            return Ok();
        }




        //api/books/allTags
        [HttpGet("allTags")]
        public async Task<IActionResult> ReturnTags(string q) //string filePath
        {
            var allTags = await _repo.GetTags(q);

            return Ok(allTags);
        }



        //api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> PerviewBook(int id) //string filePath
        {

            var theBook = await _repo.GetBook(id); // find it by Id=1

            MemoryStream ms = new MemoryStream(theBook.ByteBook);
            return new FileStreamResult(ms, "application/pdf");

        }


        //api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var theBook = await _repo.GetBook(id);

            _repo.Delete(theBook);
            if (await _repo.SaveAll())
            {
                return Ok("Done");
            }
            return Ok("not Done");
        }





        //api/books/search/{id}
        [HttpGet("search/{id}")]
        public async Task<IActionResult> SearchBook(int id) //string filePath
        {
            string libraryPath = Directory.GetCurrentDirectory();

            var reader = new PdfReader(libraryPath + "/book/hello2.pdf"); // ova ja cita TOP!
            StringWriter output = new StringWriter();

            //var theBook = await _repo.GetBook(id); // find it by Id=1


            //MemoryStream ms = new MemoryStream(theBook.ByteBook);
            //return new FileStreamResult(ms, "application/pdf");    // open the book Perview


            //for (int i = 1; i <= ms.NumberOfPages; i++)
            //    output.WriteLine(PdfTextExtractor.GetTextFromPage(ms, i, new SimpleTextExtractionStrategy()));

            //var stringBook = outPut.ToString();
            //now you can search for the text from outPut.ToString();
            ///
            ///

            Process process = new Process();


            //process.StartInfo.Arguments = "/A \"page=n\" \"F:\\STAGE\\test.pdf";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            process.StartInfo = startInfo;
            startInfo.Arguments = "/A \"page=4\"";
            startInfo.FileName = @"" + libraryPath + "/book/hello2.pdf";
            //process.Start();

            //Process.Start("book/hello2.pdf");


            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "hello2";
            string s = libraryPath + "\\book";
            //btn.Tag Contains the full file path 
            info.Arguments = "\"" + s + "\"";
            Process.Start(info);

            return Content("its okej");
        }



        //api/books/searchin/4
        [HttpGet("searchin/{id}")]
        public async Task<IActionResult> SearchInBook(int id) //string filePath
        {
            string libraryPath = Directory.GetCurrentDirectory();

            var theBook = await _repo.GetBook(id); // find it by Id=1
                                                   //System.IO.File.WriteAllBytes("book/hello2.pdf", theBook.ByteBook); // Od byte[] vo File ... ama ova go naptai vise

            var PdfDocument = new PdfReader(libraryPath + "/book/hello2.pdf"); // ova ja cita TOP!


            MemoryStream ms = new MemoryStream(theBook.ByteBook);

            Document doc = new Document(PageSize.A4, 50, 50, 15, 15);

            //PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            //PdfDocument pdf2 = new PdfDocument(writer);
            ////

            //var dest = "D:/sample.pdf";
            //var writer1 = new PdfWriter(dest);

            ////static void searchForText(string path, string text)
            ////{
            ////}
            //PdfDocument pdfR = new PdfDocument();
            //var path = libraryPath + "\\book\\hello2.pdf";
            //var text = "1";
            //using (PdfDocument pdf = new PdfDocument())
            //{
            //    pdf.PageCount(reader);

            //    for (int i = 0; i < pdf.Pages.Count; i++)
            //    {
            //        string pageText = pdf.Pages[i].GetText();
            //        int index = pageText.IndexOf(text, 0, StringComparison.CurrentCultureIgnoreCase);
            //        if (index != -1)
            //            var n = i;

            //        Console.WriteLine("'{0}' found on page {1}", text, i);
            //    }
            //}




            //System.IO.File.Delete(libraryPath + "/book/hello2.pdf"); // Delete Function Sto ne ti treba za sega

            return Ok("top");
            //return Content(najdenoVo);
        }


        //api/books/searchin/4
        [HttpGet("bytesearch/{id}")]
        public async Task<IActionResult> SearchByByte(int id) //string filePath
        {
            var searchWord = "think";
            var byteWord = Encoding.ASCII.GetBytes(searchWord);

            var theBook = await _repo.GetBook(id);
            var timesFound = Search(theBook.ByteBook, byteWord);

            if (timesFound >= 1)
            {
                return Content("a word was found " + timesFound);
            };



            return Content("we didnt get a result");
        }

        int Search(byte[] src, byte[] pattern)
        {
            int c = src.Length - pattern.Length + 1;
            int j;
            for (int i = 0; i < c; i++)
            {
                if (src[i] != pattern[0]) continue;
                for (j = pattern.Length - 1; j >= 1 && src[i + j] == pattern[j]; j--) ;
                if (j == 0) return i;
            }
            return -1;
        }



    }

}