using CrimeStats.Application.Reader;
using CrimeStats.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CrimeStats.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrimeStatsController: ControllerBase
    {
        private readonly ICrimeStatReader statReader;

        public CrimeStatsController(ICrimeStatReader statReader)
        {
            this.statReader = statReader;
        }
        [HttpGet(Name = "GetCrimeStats")]
        public async Task<IActionResult> Get(string? category = null)
        {
            if (string.IsNullOrEmpty(category))
            {
                var response = await statReader.ReadCrimeStatsAsync();
                return Ok(response);
            }
            else
            {
                var validCategories = await statReader.ReadCrimeStatCategoriesAsync();
                if (!validCategories.Contains(category))
                {
                    return BadRequest($"Invalid category '{category}'. Valid categories are: {string.Join(", ", validCategories)}");
                }
                var response = await statReader.ReadCrimeStatsAsync(category);
                return Ok(response);
            }
        }
    }
}
