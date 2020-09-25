using BookLibrary.Data;
using BookLibrary.Models;
using BookLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using PDFUpload.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFUpload.Repos
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ApplicationDbContext _context;

        public LibraryRepository(ApplicationDbContext context)
        {

            _context = context;
        }


        //  Basic Functions
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() >= 0;
        }




        //    Books

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var allBooks = await _context.Books.ToListAsync();
            return allBooks;
        }


        public async Task<Book> GetBook(int id)
        {
            var Book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            return Book;
        }

        public async Task<Tag> GetTag(int id)
        {
            var Tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            return Tag;
        }

        public async Task<IEnumerable<Tag>> GetTags(string query)
        {


            if (!String.IsNullOrWhiteSpace(query))
            {
                var filterTags = await _context.Tags.Include(t => t.BookTags).Where(m => m.TagName.Contains(query)).ToListAsync();
                return filterTags;
            }
            var Tags = await _context.Tags.ToListAsync();

            return Tags;
        }

        public async Task<Book> GetNewBook()
        {
            var newBook = await _context.Books.FirstOrDefaultAsync(b => b.IsNew == true);
            return newBook;
        }

        public async Task<IEnumerable<Tag>> TagsForNewBook()
        {
            var newBookTags = await _context.Tags.Where(m => m.ForNewBook == true).ToListAsync();
            return newBookTags;
        }
        public async Task<IEnumerable<BookTags>> FoundBookTags(List<int> SelectedTags)
        {
            var SelectedBookTags = await _context.BookTags.Where(b => SelectedTags.Any(t => t == b.TagId)).ToListAsync();

            return SelectedBookTags;
        }








        public async Task<IEnumerable<Book>> FoundBooks(IEnumerable<ViewTagModel> bookTags)
        {
            int[] int_arr = { 38431220 };
            int tag = 38431220;

            int id;
            var tagIds = new List<int>();
            foreach (var bTag in bookTags)
            {
                int.TryParse(bTag.id, out id);
                tagIds.Add(id);
            }


            //var Books = _context.Books
            //   .Include(e => e.BookTags)
            //       .ThenInclude(e => e.Tag);

            //var linqBooks = await _context.Tags.Where(t => t.Id == 38431220).Books.Select(b => b.Id)
            var Books = await _context.Books.ToListAsync();
            var repoBookTags = await _context.BookTags.ToListAsync();

            List<int> selectedTagIds = tagIds;
            //List<int> selectedTagIds = new List<int>() { 38431220, 60489222 };
            var query = from book in Books
                        join booktag in repoBookTags
                        on book.Id equals booktag.BookId
                        join selectedId in selectedTagIds
                        on booktag.TagId equals selectedId
                        group book by book into bookgroup
                        where bookgroup.Count() == selectedTagIds.Count
                        select bookgroup.Key;

            //////////////////////////////////////////
            ////////////////////////////////////////// 
            //////////////////////////////////////////
            /////
            var BooksToReturn = new List<ViewBookModel>();

            //BooksToReturn = await _context.Books.Select(book => new ViewBookModel
            //{
            //    Id = book.Id,
            //    Title = book.Title,
            //    Author = book.Author,
            //    Pages = book.Pages,
            //    Tags = book.BookTags.Select(s => s.Tag)
            //}).ToListAsync();
            //////////////////////////////////////////
            //////////////////////////////////////////
            //////////////////////////////////////////

            //var newBookTry = await BooksToReturn.Select(b => (b.Tags.Where(t => t.Id == tag)).ToList()


            //BooksToReturn = BooksToReturn.Where(b=>b.Tags == )

            //var ReturnBooks = await _context.Books.Where(b=> b.Tag.id == SearchTagId).ToListAsync()

            //var teams = db.Teams.Select(team => new {
            //    TeamName = team.Name,
            //    PlayersOlder20 = team.PlayerTeams.Where(pt => pt.Player.Age > 20).Select(s => s.Player)
            //});



            //var booksToReturn = await Books.Where(b => b.BookTags.Where(bt => bt.Tag.Id == tagid)


            //var result = _context.Books.Where(p => p.Tags.Any(s => s.Id == id)).ToList();

            //var Books = await _context.Books.Where(b => TagIds.Any(t => t == b.BookTags))
            //    .ToListAsync();

            //var SelectedBooks = await _context.Books
            //.Include(e => e.BookTags)
            //.ThenInclude(e => e.Tag).ToListAsync();

            //SelectedBooks.Where(b => TagIds.Any(t => t == b.TagId));

            //.Where(b => BookTags.All(bt => bt.BookId == b.Id)).ToListAsync(); 

            return query;
        }
    }
}
