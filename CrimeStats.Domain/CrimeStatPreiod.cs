using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeStats.Domain
{
    public class CrimeStatPreiod
    {
        public int Year { get; set; }
        public string MonthFrom { get; set; }
        public string MonthTo { get; set; }
        public int Value { get; set; }
        public int Order { get; set; }
    }
}
