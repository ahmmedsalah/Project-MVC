using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;


		public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index(string searchvalue)
		{
			var employee = Enumerable.Empty<Employee>();

			if (string.IsNullOrEmpty(searchvalue))
				 employee = await _unitOfWork.EmployeeReopository.GetAll();

			else
				 employee = _unitOfWork.EmployeeReopository.searchEmployeebyname(searchvalue);

			var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employee);
			return View(mappedEmps);



		}
		public async  Task<IActionResult> Create()
		{
			ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();

			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
		{
			
			if (ModelState.IsValid)
			{
				employeeVM.ImageName = await DocumentSettings.UploadFileAsync(employeeVM.Image, "images");
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

				TempData["message"] = "Employee is Created Susseccfull";
				await _unitOfWork.EmployeeReopository.Add(mappedEmp);
				return RedirectToAction(nameof(Index));

			}
			return View(employeeVM); 

		}
		public async Task<IActionResult> Detials(int? id, string ViewName = "Detials")
		{
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
            if (id == null)
				return NotFound();
			var employee = await _unitOfWork.EmployeeReopository.Get(id.Value);
			if (employee == null)
				return NotFound();

			var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
			return View(ViewName, MappedEmployee);

		}
		//Departemtn/Edit/1
		public async Task<IActionResult> Edit(int? id)
		{
			ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
			return await Detials(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
		{
			if (id != employeeVM.Id)
			{
				return BadRequest();
			}
			if (ModelState.IsValid)
			{
				try
				{
					employeeVM.ImageName = await DocumentSettings.UploadFileAsync(employeeVM.Image,"images");
					var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				  await	_unitOfWork.EmployeeReopository.Update(mappedEmployee);
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(employeeVM);

		}

		public async Task<IActionResult> Delete(int? id)
		{
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
            //if (id == null)
            //	return BadRequest();
            //var department = _departmentRepository.Get(id.Value);
            //if (department == null)
            //	return BadRequest();

            return await Detials(id, "Delete");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async  Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVm)
		{
			if (id != employeeVm.Id)
				return BadRequest();
			try
			{
				if (ModelState.IsValid)
				{
					
					var mapperemp = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);

					int count = await _unitOfWork.EmployeeReopository.Delete(mapperemp);
					if(count > 0)
                        DocumentSettings.DeleteFile(employeeVm.Name, "images");
                    return RedirectToAction(nameof(Index));
				}
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(employeeVm);
		}
	}
}
