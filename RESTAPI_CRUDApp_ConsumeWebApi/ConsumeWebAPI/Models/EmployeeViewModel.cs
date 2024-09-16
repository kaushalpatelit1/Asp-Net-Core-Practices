using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ConsumeWebAPI.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [Required]
        public bool IsFullTime { get; set; }
        public double Salary { get; set; }
    }
}
