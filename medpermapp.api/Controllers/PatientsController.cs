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
            var patients = await _context.Patients
            .Include(pat => pat.Address)
                .ThenInclude(add => add.City)
            .Include(pat => pat.Address)
                .ThenInclude(add => add.County)
            .Include(pat => pat.Address)
                .ThenInclude(add => add.Country)
            .ToListAsync();
            foreach(var patient in patients) {
                System.Console.WriteLine(patient);
            }
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id) 
        {
            var value = await _context.Patients.Include(patient => patient.Address)
            .Include(pat => pat.Address)
                .ThenInclude(add => add.City)
            .Include(pat => pat.Address)
                .ThenInclude(add => add.County)
            .Include(pat => pat.Address)
                .ThenInclude(add => add.Country)
            .FirstOrDefaultAsync(patient => patient.Id == id);

            return Ok(value);
        }
    }
}