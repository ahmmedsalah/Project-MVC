using Demo.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Code is Required")]
        [MaxLength(50, ErrorMessage = "Maxlength is 50")]
        public string Name { get; set; }
		public DateTime DateOfCreation { get; set; }
		//NavigationProperty[Many]
		public ICollection<Employee> Employees { get; set; }
    }
}
