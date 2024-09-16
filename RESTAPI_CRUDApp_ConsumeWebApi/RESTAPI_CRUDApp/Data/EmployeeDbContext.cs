using Microsoft.EntityFrameworkCore;
using RESTAPI_CRUDApp.Models;

namespace RESTAPI_CRUDApp.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
