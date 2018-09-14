using AGRol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRol.Models
{
    public class CharSkill
    {
        public int id;
        public int exp = 0;
        public int lv = 0;
        public int nextlv = 10;

        public bool addExp(int exp)
        {
            bool hasleveled = false;
            while (exp >= nextlv)
            {
                hasleveled = true;
                exp -= nextlv;
                levelUp();
            }
            this.exp += exp;
            nextlv -= exp;
            return hasleveled;
        }

        public void levelUp()
        {
            exp = 0;
            lv++;
            nextlv = 10 + (int)Math.Pow((double)lv, (double)2);
        }
        public string getName()
        {
            return MainMethods.skills[this.id].name;
        }

        public override string ToString()
        {
            return getName() + ": Lv " + lv + ". " + nextlv + " to level up!";
        }
    }
}
