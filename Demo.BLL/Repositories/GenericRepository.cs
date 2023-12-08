using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly MvcApplicationDbContext _dbcontext;

        public GenericRepository(MvcApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

		public async Task<int> Add(T item)
		{
			await _dbcontext.Set<T>().AddAsync(item);
			return await _dbcontext.SaveChangesAsync();
		}

		public async Task<int> Delete(T item)
		{
		   	_dbcontext.Set<T>().Remove(item); 
			return await _dbcontext.SaveChangesAsync();

		}

		public async Task<T> Get(int id)
		=> await _dbcontext.Set<T>().FindAsync(id);

		public async Task<IEnumerable<T>> GetAll()
		{
			if(typeof(T)==typeof(Employee))
			  return (IEnumerable<T>) await _dbcontext.Set<Employee>().Include(D=>D.Department).ToListAsync();

			return await _dbcontext.Set<T>().ToListAsync();
		}

		public async Task<int> Update(T item)
		{
		  	_dbcontext.Set<T>().Update(item);
			return await _dbcontext.SaveChangesAsync();
		}
	}
}
