using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESTAPI_CRUDApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [Required]
        public bool IsFullTime { get; set; }
        public double Salary { get; set; }
        
    }
}
