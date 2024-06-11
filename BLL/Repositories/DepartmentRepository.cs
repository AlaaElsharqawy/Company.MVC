using BLL.Interfaces;
using DAL.Contexts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class DepartmentRepository :GenericRepository<Department> ,IDepartmentRepository
    {

        public DepartmentRepository(MvcAppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
