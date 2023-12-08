using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Demo.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}


		public async Task<IActionResult> Index(string searchvalue)
		{
			var users = Enumerable.Empty<ApplicationUser>().ToList();

			if (string.IsNullOrEmpty(searchvalue))
				users.AddRange(_userManager.Users);

			else

				users.Add(await _userManager.FindByEmailAsync(searchvalue));
			return View(users);


		}
		//public async Task<IActionResult> Create()
		//{
		//	ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();

		//	return View();
		//}
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
		//{

		//	if (ModelState.IsValid)
		//	{
		//		employeeVM.ImageName = await DocumentSettings.UploadFileAsync(employeeVM.Image, "images");
		//		var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

		//		TempData["message"] = "Employee is Created Susseccfull";
		//		await _unitOfWork.EmployeeReopository.Add(mappedEmp);
		//		return RedirectToAction(nameof(Index));

		//	}
		//	return View(employeeVM);

		//}
		public async Task<IActionResult> Detials(string id, string ViewName = "Detials")
		{

			if (id == null)
				return NotFound();
			var User = await _userManager.FindByIdAsync(id);
			if (User == null)
				return NotFound();
			return View(ViewName, User);

		}
		//Departemtn/Edit/1
		public async Task<IActionResult> Edit(string id)
		{

			return await Detials(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] string id, ApplicationUser updatedUser)
		{
			if (id != updatedUser.Id)
			{
				return BadRequest();
			}
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userManager.FindByIdAsync(id);

					user.UserName = updatedUser.UserName;

					user.PhoneNumber = updatedUser.PhoneNumber;

					await _userManager.UpdateAsync(user);

					return RedirectToAction(nameof(Index));

				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(updatedUser);

		}

		public async Task<IActionResult> Delete(string id)
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
		public async Task<IActionResult> Delete([FromRoute] string id, ApplicationUser deletedUser)
		{
			if (id != deletedUser.Id)
				return BadRequest();
			try
			{
				var user = await _userManager.FindByIdAsync(id);

				await _userManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(deletedUser);
		}
	}
}
