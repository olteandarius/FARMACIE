using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Book> GetById(Guid id)
        {
            return await _dbSet.Include(t => t.Owner).SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<List<Book>> List()
        {
            /*
            var filtered = from e in _dbSet
                           where e.Author.Contains(author)
                           select e;
            */
            return await _dbSet.Include(t => t.Owner).ToListAsync();
        }
    }
}
