using Demo.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		public IEmployeeReopository EmployeeReopository { get; set; }
		public IDepartmentRepository DepartmentRepository { get; set; }

        public UnitOfWork(IEmployeeReopository employeeRepository, IDepartmentRepository departmentRepository)
        {
			EmployeeReopository = employeeRepository;
			DepartmentRepository = departmentRepository;
		}
    }
}
