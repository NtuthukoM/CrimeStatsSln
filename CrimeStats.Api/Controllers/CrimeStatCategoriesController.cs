using CrimeStats.Application.Reader;
using Microsoft.AspNetCore.Mvc;

namespace CrimeStats.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrimeStatCategoriesController: ControllerBase
    {
        private readonly ICrimeStatReader statReader;

        public CrimeStatCategoriesController(ICrimeStatReader statReader)
        {
            this.statReader = statReader;
        }

        //add route to get crime stat categories
        [HttpGet(Name = "GetCrimeStatCategories")]
        public async Task<IEnumerable<string>> GetCategories()
        {
            return await statReader.ReadCrimeStatCategoriesAsync();
        }
    }
}
