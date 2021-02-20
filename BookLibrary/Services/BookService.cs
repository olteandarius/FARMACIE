using BookLibrary.Models;
using BookLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> List();
        Task<List<Book>> ListForAuthor(string author);
        Task Remove(Guid id);
        Task Insert(Book book);
        Task Update(Book book);
        Task<Book> GetById(Guid? id);
        Task<List<MinMaxPricesPerYearViewModel>> GetMinMaxPricesPerYearReport();
    }

    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> List()
        {
            return await _bookRepository.List();
        }

        public async Task Remove(Guid id)
        {
            await _bookRepository.Remove(id);
            await _bookRepository.Save();
        }

        public async Task Insert(Book book)
        {
            book.Id = Guid.NewGuid();
            await _bookRepository.Insert(book);
            await _bookRepository.Save();
        }

        public async Task Update(Book book)
        {
            _bookRepository.Update(book);
            await _bookRepository.Save();
        }

        public async Task<Book> GetById(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _bookRepository.GetById(id.Value);
        }

        public async Task<List<Book>> ListForAuthor(string author)
        {
            List<Book> allBooks = await List();
            return allBooks.Where(b => b.Author.ToLower().Contains(author.ToLower())).ToList();
        }

        public async Task<List<MinMaxPricesPerYearViewModel>> GetMinMaxPricesPerYearReport()
        {
            var allBooks = await List();
            return
                allBooks
                .GroupBy(b => b.PublicationYear)
                .Select(g => new MinMaxPricesPerYearViewModel
                {
                    PublicationYear = g.Key,
                    MinPrice = g.Min(b => b.Price),
                    MaxPrice = g.Max(b => b.Price),
                    MinPriceTitle = g.FirstOrDefault(b => b.Price == g.Min(bb => bb.Price)).Title,
                    MaxPriceTitle = g.FirstOrDefault(b => b.Price == g.Max(bb => bb.Price)).Title,
                })
                .OrderBy(b => b.PublicationYear)
                .ToList();
        }
    }
}
