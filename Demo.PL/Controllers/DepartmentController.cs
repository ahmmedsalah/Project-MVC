using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
	{
		private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository,IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
		{
			var department = await _departmentRepository.GetAll();
			var mapperDepart = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
			return View(mapperDepart);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
           if (ModelState.IsValid)
			{
				var mapperDepart = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
			   await _departmentRepository.Add(mapperDepart);
				return RedirectToAction(nameof(Index));

			}
		   return View(departmentVM);

        }
        public async Task<IActionResult> Detials(int? id,string ViewName = "Detials")
		{
			if (id == null)
				return NotFound();
			var department = await _departmentRepository.Get(id.Value);
			if (department == null)
				return NotFound();
			var mapperdepart = _mapper.Map<Department,DepartmentViewModel>(department);
			return View(ViewName, mapperdepart);

		}
		//Departemtn/Edit/1
		public async Task<IActionResult> Edit(int? id)
		{
			return await Detials(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel departmentVM)
		{
			if (id!= departmentVM.Id)
			{
				return BadRequest();
			}
			if (ModelState.IsValid)
			{
				try
				{
					var mapperDepart = _mapper.Map<DepartmentViewModel,Department>(departmentVM);	
				await _departmentRepository.Update(mapperDepart);
				return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(departmentVM);

		}

		public async Task<IActionResult> Delete (int? id)
		{
			//if (id == null)
			//	return BadRequest();
			//var department = _departmentRepository.Get(id.Value);
			//if (department == null)
			//	return BadRequest();

			return await Detials(id, "Delete");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete ([FromRoute] int id, DepartmentViewModel departmentVM)
		{
			if (id != departmentVM.Id)
				return BadRequest();
			try
			{
				if (ModelState.IsValid)
				{
					var mapperDepart = _mapper.Map<DepartmentViewModel,Department>(departmentVM);
				  await	_departmentRepository.Delete(mapperDepart);
					return RedirectToAction(nameof(Index));
				}
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(departmentVM);
		}
	}
}
