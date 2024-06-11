using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexts
{
  public class MvcAppDbContext:IdentityDbContext<ApplicationUser>
    {
        public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options):base(options)
        {
            
        }

      

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
