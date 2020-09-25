﻿using BookLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

            //int id;
            //var tagIds = new List<int>();

            //foreach (var tag in bookTags)
            //{
            //    int.TryParse(tag.id, out id);
            //    tagIds.Add(id);
            //}

            ////tagIds.AddRange(bookTags);
            ////int[] int_arr = { 60487999, 60489222 };



            //var returnBooks = new List<Book>();

            //var Books = await _repo.FoundBooks(tagIds);

            //Books.Select(b => b.Tag)

            //foreach (var searchTag in tagIds)
            //{
            //    foreach (var book in Books)
            //    {
            //        foreach (var booktag in book.BookTags)
            //        {
            //            if (booktag.TagId == searchTag)
            //            {
            //                returnBooks.Add(book);
            //                break;

            //            }

            //        }

            //    }
            //}

            //var dtoBooks = new List<BookListDto>();

            //foreach (var book in returnBooks)
            //{
            //    var newBookDto = new BookListDto()
            //    {
            //        Id = book.Id,
            //        Pages = book.Pages,
            //        Author = book.Author,
            //        Title = book.Title,

            //    };
            //    dtoBooks.Add(newBookDto);
            //}


            //List<BookListDto> noDuplicateDtoBookTags = dtoBooks.Distinct().ToList();


            var Books = await _repo.FoundBooks(bookTags);


            return Ok(Books);
        }














        // Create       /// BRISI
        // POST     /api/Search/FindBooks
        [HttpPost("FindBooks")]
        public async Task<IActionResult> FindBooks(List<int> SelectedTags)
        {


            var bookTags = await _repo.FoundBookTags(SelectedTags); // ke mi gi vrati site bookTags koi go sodrzat nasite TagIds

            var BookIds = new List<int>();

            //var BooksToReturn = _repo.FoundBooks(bookTags); /// Ok e ova!







            // Vidi so Include _contex.Book.Include(b=>b.BookTags)

            //var bookList = _repo.GetBooks().where(b => b.id) za brisenje

            //    var result = peopleList2.Where(p => !peopleList1.Any(p2 => p2.ID == p.ID));

            //var result = peopleList2.Where(p => peopleList1.All(p2 => p2.ID != p.ID)); vidi go ova bidejki i 2te resenija se dobri



            return Ok(bookTags); // ubavo ke bide da vratis DtoListBook bidejki ne mora Byre[] da go setas

        }





    }
}