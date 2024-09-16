using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfService.Server.MyWcf.Contract;

namespace WcfService.Server
{
    public class MyWcfService : IMyWcfService
    {
        private SqlConnection _sqlConnection;
        private string _query;
        public MyWcfService()
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["KpConnection"].ToString());
            if(_sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            var employeeList = new List<Employee>();
            _query = $"Select Id, FirstName, LastName, DateOfBirth, Email, Salary from Employees";
            SqlCommand command = new SqlCommand(_query, _sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.FieldCount > 0)
            {
                while(reader.Read())
                {
                    Employee employee = new Employee()
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateOfBirth = reader.GetDateTime(3),
                        Email = reader.GetString(4),
                        Salary = reader.GetDouble(5)
                    };
                    employeeList.Add(employee);
                }
            }
            CloseSqlConnection();
            return employeeList;
        }
        protected internal void CloseSqlConnection()
        {
            _sqlConnection.Close();
        }
    }
}
