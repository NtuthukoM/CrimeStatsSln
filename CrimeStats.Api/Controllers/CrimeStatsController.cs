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
        public async Task<IEnumerable<CrimeStat>> Get()
        {
           return await statReader.ReadCrimeStatsAsync();
        }
    }
}
