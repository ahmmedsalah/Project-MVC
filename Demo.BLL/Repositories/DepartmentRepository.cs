using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
	{
        public DepartmentRepository(MvcApplicationDbContext dbContext):base(dbContext)
        {
            
        }

    }
}
