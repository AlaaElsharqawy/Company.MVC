 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
   public class Department
    {
       //EF ByConvention in .net 5

        public int Id { get; set; }//PK

        // any change by data annotation
        [Required(ErrorMessage ="Name Is Required")]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }//nVarchar(max) +null


        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }

        public DateTime DateOfCreation { get; set; }

        [InverseProperty("Department")]

        //Navigational property[Many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
      
    }
}
