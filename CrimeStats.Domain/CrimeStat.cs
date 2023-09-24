using System.Net.Http.Headers;

namespace CrimeStats.Domain
{
    public class CrimeStat
    {
        public string Catergory { get; set; }
        public string SubCategory { get; set; }
        public List<CrimeStatPreiod> CrimeStatPreiods { get; set; }
        public int CountDiff { get 
            {
                int count = 0;
                //Return the difference between the second last and last value from the CrimeStatPreiods list ordered by 'Order':
                if(CrimeStatPreiods != null && CrimeStatPreiods.Count > 1)
                {
                    count = CrimeStatPreiods.OrderByDescending(x => x.Order).Take(2).Select(x => x.Value).Aggregate((x, y) => x - y);
                }

                return count;
            } 
        }
    }
}