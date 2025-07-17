using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigortaYonetimAPI.Models;

namespace SigortaYonetimAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KullanicilarController : ControllerBase
    {
        private readonly SigortaYonetimDbContext _context;

        public KullanicilarController(SigortaYonetimDbContext context)
        {
            _context = context;
        }

        // GET: api/Kullanicilar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KULLANICILAR>>> GetKullanicilar()
        {
            return await _context.KULLANICILARs.ToListAsync();
        }

        // GET: api/Kullanicilar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KULLANICILAR>> GetKullanici(int id)
        {
            var kullanici = await _context.KULLANICILARs.FindAsync(id);

            if (kullanici == null)
            {
                return NotFound();
            }

            return kullanici;
        }
    }
}