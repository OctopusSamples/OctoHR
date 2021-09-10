using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OctopusSamples.OctoHR.PublicWebApp.Data;
using System.Collections.Generic;
using System.Linq;

namespace OctopusSamples.OctoHR.PublicWebApp.Pages.Employees
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly ILogger<ListModel> _logger;
        private readonly IEmployeeService _employeeService;

        public ListModel(ILogger<ListModel> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public List<Employee> Employees { get; set; }

        public IActionResult OnGet()
        {
            var clientCode = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ClientCode")?.Value;
            if (!string.IsNullOrWhiteSpace(clientCode))
            {
                Employees = _employeeService.GetEmployees(clientCode);
                if (Employees == null)
                {
                    return RedirectToPage("/Error");
                }
                
                return Page();
            }

            return RedirectToPage("/Error");
        }
    }
}
