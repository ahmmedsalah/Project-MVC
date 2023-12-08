﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
	public class Employee
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		public int? Age { get; set; }
		public string Address { get; set; }
		[DataType(DataType.Currency)]
		public decimal Salary { get; set; }

		public bool IsActive { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		//ForeignKey("Department")
        public int? DepartmentId { get; set; }
        //NavigationProperty[one]
        public Department Department { get; set; }

	    public string ImageName { get; set; }


    }
}
