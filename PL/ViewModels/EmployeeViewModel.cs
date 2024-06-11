﻿using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace PL.ViewModels
{
    public class EmployeeViewModel
    {


        public int Id { get; set; }//PK



        //FrontEnd Validation

        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min Length is 50 Chars")]
        public string Name { get; set; }



        [Range(22, 35, ErrorMessage = "Age Must be In Range From 22 To 35")]
        public int? Age { get; set; }



        //[RegularExpression("^[0-9]{1,3}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",ErrorMessage ="Address Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }



        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }


        public bool IsActive { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        [Phone]
        public string PhoneNumber { get; set; }



        public DateTime HireDate { get; set; }

        //Image
       
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }
      

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }//FK

        [InverseProperty("Employees")]
        public Department Department { get; set; }//navigational property[one]


    }
}
