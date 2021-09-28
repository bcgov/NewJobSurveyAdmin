using Microsoft.AspNetCore.Mvc;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvExtractController : ControllerBase
    {
        private readonly NewJobSurveyAdminContext context;
        private readonly CsvService csv;
        private readonly EmployeeReconciliationService employeeReconciler;
        private readonly LoggingService logger;

        public CsvExtractController(
            NewJobSurveyAdminContext context,
            CsvService csv,
            EmployeeReconciliationService employeeReconciler,
            LoggingService logger
        )
        {
            this.context = context;
            this.csv = csv;
            this.employeeReconciler = employeeReconciler;
            this.logger = logger;
        }

        // EmployeesFromCsv: Given the raw text of the PSA Csv extract (as
        // obtained, for instance, from the PSA CSV file drop), transform it
        // into an array of nicely-formatted Employee JSON objects, then
        // reconcile each of those Employees.
        // POST: api/CsvExtract/EmployeesFromCsv
        [HttpPost("EmployeesFromCsv")]
        public async Task<ActionResult<List<Employee>>> EmployeesFromCsv()
        {
            var employeeList = new List<Employee>();

            try
            {
                // Get a list of candidate Employee objects based on the CSV.
                employeeList = await csv.ProcessCsv(Request, employeeReconciler, logger);
            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.ReconcileCsv,
                    $"Error reconciling employee records. Stacktrace:\r\n" +
                    e.StackTrace
                );
            }

            return Ok(employeeList);
        }
    }
}