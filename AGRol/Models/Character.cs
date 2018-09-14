using AGRol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRol.Models
{
    public class Character
    {
        public enum Alignments { Law, Neutrality, Chaos }

        public string pname;
        public string dmname;
        public string cname;
        public bool unlocked;
        public Alignments alignment;
        public Stats stats;
        public int species;
        public int money;
        public int maxlanguages;
        public string hpower;
        public string quirk;
        public Dictionary<string, bool> languages;

        public void resetCharacter()
        {
            pname = string.Empty;
            dmname = string.Empty;
            cname = string.Empty;
            unlocked = false;
            alignment = Alignments.Neutrality;

            stats = new Stats();
            stats.resetStats();

            species = 0;
            money = 0;
            maxlanguages = 2;
            hpower = string.Empty;
            quirk = "None";

            languages = new Dictionary<string, bool>();
            languages["Antroz"] = true;
            languages["Gatusian"] = false;
            languages["Rokrok"] = false;
            languages["God"] = false;
        }

        public override string ToString()
        {
            return "Player " + pname + ": " + cname + ", " + MainMethods.specs[species].name;
        }
    }
}
