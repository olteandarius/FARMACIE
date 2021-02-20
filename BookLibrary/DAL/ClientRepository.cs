using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.DAL
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Client> GetById(Guid id)
        {
            return await _dbSet.Include(t => t.Owner).SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<List<Client>> List()
        {
            return await _dbSet.Include(t => t.Owner).ToListAsync();
        }
    }
}
