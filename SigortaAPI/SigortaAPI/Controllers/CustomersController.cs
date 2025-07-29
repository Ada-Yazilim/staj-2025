using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaAPI.Data;
using SigortaAPI.Models;

namespace SigortaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Tüm metodlar için token gereksinimi
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/customers
        [HttpGet]
        [Authorize(Roles = "Agent,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Customers.ToListAsync();
            return Ok(list);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        [Authorize(Roles = "Agent,Admin")]
        public async Task<IActionResult> Create([FromBody] Customer model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Customers.Add(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Agent,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer model)
        {
            if (id != model.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Customers.FindAsync(id);
            if (entity == null) return NotFound();

            _db.Customers.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
