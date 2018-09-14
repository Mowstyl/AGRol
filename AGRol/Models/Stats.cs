using AGRol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AGRol.Models
{
    public class Stats
    {
        public enum Abilities { Strength, Intelligence, Wisdom, Dexterity, Constitution, Charisma };
        public enum Saving { PoisonOrDRay, MagicWand, TurnStoneOrPar, DrBreath, SpellorMStaff };

        public int[] scores;
        public int[] rbonus;
        public int hp;
        public int exp;
        public int level;
        public int nextlv;
        public CharSkill[] skills;

        public void resetStats()
        {
            scores = new int[6];
            rbonus = new int[6];
            hp = 1;
            exp = 0;
            level = 1;
            nextlv = 100;
            skills = new CharSkill[MainMethods.skills.Length];

            for (int i = 0; i < 6; i++)
            {
                scores[i] = 9;
            }

            for (int i = 0; i < MainMethods.skills.Length; i++)
            {
                skills[i] = new CharSkill { id = i };
            }
        }

        public void addExp(int exp, MainMethods.SkillEnum sk)
        {
            if (skills[(int)sk].addExp(exp))
            {
                MessageBox.Show(skills[(int)sk].getName() + "is now at level " + skills[(int)sk].lv, "Skill level up!");
            }
            bool hasleveled = false;
            while (exp >= nextlv)
            {
                hasleveled = true;
                exp -= nextlv;
                levelUp();
            }
            this.exp += exp;
            nextlv -= exp;
            if (hasleveled)
            {
                MessageBox.Show("The character is now at level " + level, "Level up!");
            }
        }

        public void levelUp()
        {
            exp = 0;
            level++;
            nextlv = 100 + (int)Math.Pow((double)level, (double)3); ;
        }
    }
}
