using System.Runtime.Serialization;

namespace MVCWcf.Client.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }
        public string DisplayName => GetDisplayName();
        private string GetDisplayName()
        {
            return FirstName + " " + LastName;
        }

        public void Import(MyWcfServiceReference.Employee dto)
        {
            Id = dto.Id;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            DateOfBirth = dto.DateOfBirth;
            Email = dto.Email;
            Salary = dto.Salary;
        }
        public MyWcfServiceReference.Employee Export()
        {
            MyWcfServiceReference.Employee wcfEmp = new MyWcfServiceReference.Employee()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                DateOfBirth = this.DateOfBirth,
                Email = this.Email,
                Salary = this.Salary,
            };
            return wcfEmp;
        }
    }
}
