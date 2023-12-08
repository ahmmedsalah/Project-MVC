using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeReopository
	{
		private readonly MvcApplicationDbContext _dbContext;

        public EmployeeRepository(MvcApplicationDbContext dbContext) : base(dbContext) 
        {
			_dbContext = dbContext;
		}
        public IQueryable<Employee> GetEmployeesByDepartment(string department)
		{
			throw new NotImplementedException();
		}

		public IQueryable<Employee> searchEmployeebyname(string name)
		=> _dbContext.Employees.Where(E => E.Name.Contains(name));
	}
}
