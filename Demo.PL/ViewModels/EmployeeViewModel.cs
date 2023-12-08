using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required!")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        [MinLength(5, ErrorMessage = "MinLength is 5")]
        public string Name { get; set; }
        [Range(18, 30, ErrorMessage = "Age Must be between 22 to 30 ")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
          , ErrorMessage = "Address must be like 123-Street-City-Country")]

        public string Address { get; set; }
        [DataType(DataType.Currency)]
        [Range(4000, 8000)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
        [EmailAddress(ErrorMessage ="Email must be @gmail.com")]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        //ForeignKey("Department")
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        //NavigationProperty[one]
        public Department Department { get; set; }

        public IFormFile Image { get; set; }
      
        public string ImageName { get; set; }

    }
}
