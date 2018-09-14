using AGRol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRol.Models
{
    public class Species
    {
        public MainMethods.SpecEnum type { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public int[] adjust { get; set; }
        public int[] sThrows { get; set; }
        public int[] life { get; set; }
        public bool canDistr { get; set; }
        public int forceAlign { get; set; }
        public List<string> hpower { get; set; }
        public Func<int, int, int, int> hpcalc { get; set; }

        public int[] getRangeHP()
        {
            return new int[2] { life[0], life[0] * life[1] };
        }
    }
}
