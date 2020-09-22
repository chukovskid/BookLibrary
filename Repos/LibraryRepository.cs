using BookLibrary.Data;
using BookLibrary.Models;
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








        public async Task<IEnumerable<Book>> FoundBooks(IEnumerable<int> TagIds)
        {
            var id = 25560611;
            //var result = _context.Tags
            //      .Where(t => t.Id == id)
            //      .SelectMany(t => t.Books)
            //      .ToList();

            //var Books = _context.Books.Select(o => new
            //{
            //    o.Id,
            //    Tags = o.BookTags.Select(ot => ot.Tag).ToList()
            //}).ToList();



            //var Books =  _context.Books.Include(t => t.BookTags).ThenInclude(bt => bt.Tag)
            //                    .FirstOrDefault(k => k.BookTags. == id);

            //var bookList = Tag.BookTags.Select(bt => bt.Tag).ToList();




            var Books = await _context.Books
               .Include(e => e.BookTags)
                   .ThenInclude(e => e.Tag)
               .ToListAsync();





            //var result = _context.Books.Where(p => p.Tags.Any(s => s.Id == id)).ToList();

            //var Books = await _context.Books.Where(b => TagIds.Any(t => t == b.BookTags))
            //    .ToListAsync();

            //var SelectedBooks = await _context.Books
            //.Include(e => e.BookTags)
            //.ThenInclude(e => e.Tag).ToListAsync();

            //SelectedBooks.Where(b => TagIds.Any(t => t == b.TagId));

            //.Where(b => BookTags.All(bt => bt.BookId == b.Id)).ToListAsync(); 

            return Books;
        }
    }
}
