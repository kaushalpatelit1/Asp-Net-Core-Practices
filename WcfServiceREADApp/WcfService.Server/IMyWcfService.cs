using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfService.Server.MyWcf.Contract;

namespace WcfService.Server
{
    [ServiceContract]
    public interface IMyWcfService
    {
        [OperationContract]
        IEnumerable<Employee> GetAllEmployees();
    }
}
