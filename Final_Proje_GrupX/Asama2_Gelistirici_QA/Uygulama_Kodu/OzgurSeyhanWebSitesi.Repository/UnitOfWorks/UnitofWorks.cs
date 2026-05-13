using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Repository.UnitOfWorks
{
    public class UnitofWorks(AppDbContext context) : IUnitOfWorks
    {
        private readonly AppDbContext _context=context;

        public void Commit()
        {
           _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
