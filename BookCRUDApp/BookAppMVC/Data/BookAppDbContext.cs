using BookAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAppMVC.Data
{
    public class BookAppDbContext : DbContext
    {
        public BookAppDbContext(DbContextOptions<BookAppDbContext> options)
            : base(options)
        {
            
        }
        //this will create a table called Categories
        public DbSet<Category> Categories { get; set; }
    }
}
