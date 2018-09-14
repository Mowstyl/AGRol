using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRol.Models
{
    public class CharPerk
    {
        public int id;
        public int lv = 0;

        public int getSkillId()
        {
            return id / 3;
        }
    }
}
