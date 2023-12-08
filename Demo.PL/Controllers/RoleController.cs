 using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Demo.PL.Controllers
{
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager )
        {
			_roleManager = roleManager;
		}
		public async Task<IActionResult> Index(string searchvalue)
		{
			var roles = Enumerable.Empty<IdentityRole>().ToList();

			if (string.IsNullOrEmpty(searchvalue))
				roles.AddRange(_roleManager.Roles);

			else

				roles.Add(await _roleManager.FindByNameAsync(searchvalue));
			return View(roles);


		}


		public async Task<IActionResult> Create()
		{

			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(IdentityRole role)
		{

			if (ModelState.IsValid)
			{
		          await _roleManager.CreateAsync(role);
				return RedirectToAction(nameof(Index));

			}
			return View(role);

		}
		public async Task<IActionResult> Detials(string id, string ViewName = "Detials")
		{

			if (id == null)
				return NotFound();
			var Role = await _roleManager.FindByIdAsync(id);
			if (Role == null)
				return NotFound();
			return View(ViewName, Role);

		}
		//Departemtn/Edit/1
		public async Task<IActionResult> Edit(string id)
		{

			return await Detials(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] string id, IdentityRole updatedRole)
		{
			if (id != updatedRole.Id)
			{
				return BadRequest();
			}
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _roleManager.FindByIdAsync(id);

					user.Name = updatedRole.Name;

					
					await _roleManager.UpdateAsync(user);

					return RedirectToAction(nameof(Index));

				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(updatedRole);

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
		public async Task<IActionResult> Delete([FromRoute] string id, IdentityRole deletedRole)
		{
			if (id != deletedRole.Id)
				return BadRequest();
			try
			{
				var user = await _roleManager.FindByIdAsync(id);

				await _roleManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(deletedRole);
		}
	}
}
