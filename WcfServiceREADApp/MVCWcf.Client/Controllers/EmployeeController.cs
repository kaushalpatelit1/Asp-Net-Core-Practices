using Microsoft.AspNetCore.Mvc;
using MyWcfServiceReference;
using Employee = MVCWcf.Client.Models.Employee;

namespace MVCWcf.Client.Controllers
{
    public class EmployeeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var mvcEmpList = new List<Employee>();
            MyWcfServiceClient client = new MyWcfServiceClient();
            var wcfEmpList = await client.GetAllEmployeesAsync();

            if(wcfEmpList.Count() > 0)
            {
                for(int i=0; i<wcfEmpList.Length; i++)
                {
                    Employee mvcEmp = new Employee();
                    mvcEmp.Import(wcfEmpList[i]);
                    mvcEmpList.Add(mvcEmp);
                }
            }
            return View(mvcEmpList);
        }
    }
}
