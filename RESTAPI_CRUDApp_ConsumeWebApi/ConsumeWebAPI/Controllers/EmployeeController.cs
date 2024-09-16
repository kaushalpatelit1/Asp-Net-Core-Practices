using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ConsumeWebAPI.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44351/api");
        private readonly HttpClient _client;
        public EmployeeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            List<EmployeeViewModel> empList = new List<EmployeeViewModel>();
            using(HttpResponseMessage respone = _client.GetAsync(_client.BaseAddress + "/Employees/GetEmployees").Result)
            {
                if(respone.IsSuccessStatusCode)
                {
                    string data = respone.Content.ReadAsStringAsync().Result;
                    empList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
                }
            }
            return View(empList);
        }
    }
}
