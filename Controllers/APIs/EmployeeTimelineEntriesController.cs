using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Authorize(Policy = "UserRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTimelineEntriesController : ControllerBase
    {
        private readonly NewJobSurveyAdminContext context;

        public EmployeeTimelineEntriesController(NewJobSurveyAdminContext context)
        {
            this.context = context;
        }

        // GET: api/EmployeeTimelineEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTimelineEntry>>> GetEmployeeTimelineEntries()
        {
            return await context.EmployeeTimelineEntries
                .Include(ete => ete.EmployeeAction)
                .Include(ete => ete.EmployeeStatus)
                .ToListAsync();
        }

        // GET: api/EmployeeTimelineEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTimelineEntry>> GetEmployeeTimelineEntry(int id)
        {
            var employeeTimelineEntry = await context.EmployeeTimelineEntries
                .Include(ete => ete.EmployeeAction)
                .Include(ete => ete.EmployeeStatus)
                .FirstOrDefaultAsync(ete => ete.Id == id);

            if (employeeTimelineEntry == null)
            {
                return NotFound();
            }

            return employeeTimelineEntry;
        }

        // POST: api/EmployeeTimelineEntries
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<EmployeeTimelineEntry>> PostEmployeeTimelineEntry(
            EmployeeTimelineEntry employeeTimelineEntry)
        {
            // TODO: Do proper validation here.
            context.EmployeeTimelineEntries.Add(employeeTimelineEntry);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeTimelineEntry), new { id = employeeTimelineEntry.Id },
                employeeTimelineEntry);
        }

        private bool EmployeeTimelineEntryExists(int id)
        {
            return context.EmployeeTimelineEntries.Any(e => e.Id == id);
        }
    }
}
