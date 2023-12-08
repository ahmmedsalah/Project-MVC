using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
	public interface IEmployeeReopository : IGenericRepository<Employee>
	{
		IQueryable<Employee> GetEmployeesByDepartment(string department);

		IQueryable<Employee> searchEmployeebyname(string name);

}
}
