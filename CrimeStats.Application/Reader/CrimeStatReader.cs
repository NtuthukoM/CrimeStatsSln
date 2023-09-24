using CrimeStats.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeStats.Application.Reader
{
    public class CrimeStatReader : ICrimeStatReader
    {
        public Task<List<CrimeStat>> ReadCrimeStatsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
