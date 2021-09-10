using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctopusSamples.OctoHR.PublicWebApp.Data
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployees(string clientCode);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IClientConfigDataAccess _clientConfigDataAccess;

        public EmployeeService(IClientConfigDataAccess clientConfigDataAccess)
        {
            _clientConfigDataAccess = clientConfigDataAccess;
        }

        public List<Employee> GetEmployees(string clientCode)
        {
            try
            {
                var employees = _clientConfigDataAccess.GetEmployees(clientCode);

                return employees;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
