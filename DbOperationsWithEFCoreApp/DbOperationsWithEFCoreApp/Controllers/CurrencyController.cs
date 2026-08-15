using DbOperationsWithEFCoreApp.Data;
using DbOperationsWithEFCoreApp.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            //var result = this.appDbContext.Currencies.ToList();
            //var result  = (from currencies in appDbContext.Currencies
            //              select currencies).ToList();

            //var result = await this.appDbContext.Currencies.ToListAsync();
            var result  = await (from currencies in appDbContext.Currencies
                           select currencies).ToListAsync();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyByIdAsync([FromRoute] int id)
        {
            var result = await this.appDbContext.Currencies.FindAsync(id);
            
            return Ok(result);
        }
        [HttpGet("{name}/{description}")]
        public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
        {
            //var result = await appDbContext.Currencies.Where(
            //    x => x.Title == name &&
            //    (string.IsNullOrEmpty(description) || x.Description == description)).ToListAsync();
            var result = await appDbContext.Currencies.Where(
               x => x.Title == name &&
               (string.IsNullOrEmpty(description) || x.Description == description)).ToListAsync();

            return Ok(result);
        }
        [HttpPost("all")]
        public async Task<IActionResult> GetCurreciesByIdsAsync([FromBody] List<int> ids)
        {
            // Using DTO
            /*var result = await appDbContext.Currencies
                .Where(x=> ids.Contains(x.Id))
                .Select(x=> new CurrencyDTO
                {
                    CurrencyId = x.Id,
                    CurrencyTitle = x.Title,
                }).ToListAsync();*/
            // Anonymous Method
            var result = await appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x => new 
                {
                    CurrencyId = x.Id,
                    CurrencyTitle = x.Title,
                }).ToListAsync();
            
            //var result = await appDbContext.Currencies
            //    .Where(x => ids.Contains(x.Id))
            //    .Select(x=>new Currency()
            //    {
            //        Id = x.Id,
            //        Title = x.Title,
            //    })
            //    .ToListAsync();
            return Ok(result);
        }
    }
}
