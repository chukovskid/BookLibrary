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
            int id;
            var tagIds = new List<int>();
            foreach (var bTag in bookTags)
            {
                int.TryParse(bTag.id, out id);
                tagIds.Add(id);
            }

            var Books = await _context.Books.ToListAsync();
            var repoBookTags = await _context.BookTags.ToListAsync();

            List<int> selectedTagIds = tagIds;
            var query = from book in Books
                        join booktag in repoBookTags
                        on book.Id equals booktag.BookId
                        join selectedId in selectedTagIds
                        on booktag.TagId equals selectedId
                        group book by book into bookgroup
                        where bookgroup.Count() == selectedTagIds.Count
                        select bookgroup.Key;



            return query;
        }
    }
}
