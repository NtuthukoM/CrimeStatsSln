using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeStats.Application.Reader
{
    public interface ICrimeStatReader
    {
        Task<List<Domain.CrimeStat>> ReadCrimeStatsAsync();
        Task<List<Domain.CrimeStat>> ReadCrimeStatsAsync(string category);
        Task<List<string>> ReadCrimeStatCategoriesAsync();
    }
}
