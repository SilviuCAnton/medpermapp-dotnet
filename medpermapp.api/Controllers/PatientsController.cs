using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medpermapp.api.Data;
using medpermapp.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medpermapp.api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly DataContext _context;

        public PatientsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients() 
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id) 
        {
            var value = await _context.Patients.FirstOrDefaultAsync(patient => patient.Id == id);

            return Ok(value);
        }
    }
}