using AGRol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGRol.Utils
{
    public static class MainMethods
    {
        private static Func<int, int, int, int> hpmulcalc(int mult, int div)
        {
            Func<int, int, int, int> hpfun = (int basehp, int conAdj, int level) =>
            {
                int hp = basehp + ((basehp / 4) + 1) * (level - 1);
                if (conAdj < 0)
                {
                    hp /= -conAdj;
                }
                else
                {
                    hp += (basehp * conAdj * mult) / div;
                }
                return hp;
            };
            return hpfun;
        }

        private static Species antroz = new Species
        {
            type = SpecEnum.Antroz,
            name = "Antroz",
            adjust = new int[6] { 0, 0, 0, 0, 0, 0 },
            sThrows = new int[5] { 13, 14, 15, 16, 15 },
            life = new int[2] { 1, 8 },
            canDistr = false,
            forceAlign = -1,
            hpower = new List<string> { "None", "Cleric", "Magic User" },
            hpcalc = hpmulcalc(1, 1)
        };

        private static Species gatusian = new Species
        {
            type = SpecEnum.Gatusian,
            name = "Gatusian",
            adjust = new int[6] { 0, 1, 0, 1, -1, 1 },
            sThrows = new int[5] { 12, 13, 14, 15, 16 },
            life = new int[2] { 1, 6 },
            canDistr = true,
            forceAlign = -1,
            hpower = new List<string> { "1 - None", "2 - Chewing Gum", "3 - Elemental Wizard", "4 - Healer", "5 - Electricity", "6 - None", "7 - Magnetism", "8 - Shapeshifting" },
            hpcalc = hpmulcalc(1, 2)
        };

        private static Species kurdatrux = new Species
        {
            type = SpecEnum.Kurdatrux,
            name = "Kurdatrux",
            adjust = new int[6] { 1, 0, 1, 1, 1, -1 },
            sThrows = new int[5] { 8, 9, 10, 13, 12 },
            life = new int[2] { 1, 12 },
            canDistr = true,
            forceAlign = -1,
            hpower = new List<string> { "None", "Shaman", "Blood Wizard" },
            hpcalc = hpmulcalc(2, 1)
        };

        private static Species rokrok = new Species
        {
            type = SpecEnum.Rokrok,
            name = "Rokrok",
            adjust = new int[6] { 1, 0, -1, 0, 1, 0 },
            sThrows = new int[5] { 8, 9, 10, 13, 12 },
            life = new int[2] { 1, 8 },
            canDistr = false,
            forceAlign = 2,
            hpower = new List<string> { "None" },
            hpcalc = hpmulcalc(3, 2)
        };

        private static Species margontruk = new Species
        {
            type = SpecEnum.Margontruk,
            name = "Margontruk",
            adjust = new int[6] { 1, -1, 0, 1, 0, 0 },
            sThrows = new int[5] { 14, 15, 16, 17, 17 },
            life = new int[2] { 1, 8 },
            canDistr = false,
            forceAlign = 2,
            hpower = new List<string> { "None" },
            hpcalc = hpmulcalc(1, 1)
        };

        private static Species schga = new Species
        {
            type = SpecEnum.Schga,
            name = "Sch'ga Hybrid",
            adjust = new int[6] { 0, 1, 0, 0, 1, -1 },
            sThrows = new int[5] { 17, 11, 12, 15, 14 },
            life = new int[2] { 1, 8 },
            canDistr = false,
            forceAlign = 2,
            hpower = new List<string> { "None" },
            hpcalc = hpmulcalc(3, 2)
        };

        public static int calcAdjustment(int score)
        {
            int adj;

            if (score >= 18)
            {
                adj = score - 15;
            }
            else if (score >= 16)
            {
                adj = 2;
            }
            else if (score >= 13)
            {
                adj = 1;
            }
            else if (score >= 9)
            {
                adj = 0;
            }
            else if (score >= 6)
            {
                adj = -1;
            }
            else if (score >= 4)
            {
                adj = -2;
            }
            else
            {
                adj = score - 6;
            }

            return adj;
        }

        public enum SkillEnum { Espadachin, Kendo, Jidai, Medieval, Maza, Cuerpo, ALigera, APesada, Asesino, POculto, Melee, Lanzador, Tirador }

        public enum SpecEnum { Antroz, Gatusian, Kurdatrux, Rokrok, Margontruk, Schga }

        public enum StatEnum { STR, INT, WIS, DEX, CON, CHA }

        public static Species[] specs = new Species[6] { antroz, gatusian, kurdatrux, rokrok, margontruk, schga };

        public static Perk[] perks = new Perk[]
        {
            new Perk
            {
                id = 0,
                name = "Falta daño",
                desc = "Pasiva. Aumenta un (LVPERK * 20)% el daño realizado con armas cortantes",
                lv = 0,
                reqLv = new int[5] { 4, 8, 12, 16, 20 }
            },
            new Perk
            {
                id = 1,
                name = "Contraataque",
                desc = "Activa. Se puede usar un máximo de LVPERK veces por combate cuando un ataque contra el usuario falla.",
                lv = 0,
                reqLv = new int[5] { 2, 6, 10, 14, 18 }
            },
            new Perk
            {
                id = 2,
                name = "Cuchilla",
                desc = "Pasiva. El daño aumenta en un (LVPERK * 20)% tras afilar, perdiendo 20% con cada golpe que se de con el arma hasta volver a la normalidad. No se stackea.",
                lv = 0,
                reqLv = new int[5] { 6, 10, 14, 18, 22 }
            },
            new Perk
            {
                id = 3,
                name = "Tajo sangrante",
                desc = "Activa. Golpeas haciendo un 75% del daño del ataque, y el enemigo se desangra perdiendo un 5% de su salud máxima durante (LVPERK + 2) turnos. Destreza.",
                lv = 0,
                reqLv = new int[5] { 6, 10, 14, 18, 22 }
            },
            new Perk
            {
                id = 4,
                name = "Corte vital",
                desc = "Activa. Te concentras durante un turno, y el siguiente golpe hace un 50/60/70/80/100% más de daño, sea cual sea. Destreza.",
                lv = 0,
                reqLv = new int[5] { 4, 8, 12, 16, 20 }
            },
            new Perk
            {
                id = 5,
                name = "Iai-do",
                desc = "Activa. Al desenvainar la espada golpeas haciendo un daño un (LVPERK * 20)% superior a un golpe normal.",
                lv = 0,
                reqLv = new int[5] { 2, 6, 10, 14, 18 }
            },
            new Perk
            {
                id = 6,
                name = "Reflejo",
                desc = "Activa. Rebota un proyectil. Puede usarse LVPERK veces en combate.",
                lv = 0,
                reqLv = new int[5] { 2, 6, 10, 14, 18 }
            },
            new Perk
            {
                id = 7,
                name = "Percepción aumentada",
                desc = "Activa. Te concentras durante un turno y resta LVPERK al AC del jugador.",
                lv = 0,
                reqLv = new int[5] { 4, 8, 12, 16, 20 }
            },
            new Perk
            {
                id = 8,
                name = "Erotismo y sensualidad",
                desc = "Activa, una vez cada LVPERK + 1 turnos. El enemigo se distrae y se le resta LVPERK al dado de probabilidad de acertar en su siguiente golpe.",
                lv = 0,
                reqLv = new int[5] { 6, 10, 14, 18, 22 }
            },
            new Perk
            {
                id = 9,
                name = "Bloqueo",
                desc = "Activa. El arma se puede usar como escudo.",
                lv = 0,
                reqLv = new int[1] { 5 }
            },
            new Perk
            {
                id = 10,
                name = "Berserkr",
                desc = "Pasiva. Se suma (LVPERK + 1) * 5 % al daño por cada HP que falte a la vida máxima.",
                lv = 0,
                reqLv = new int[5] { 4, 8, 12, 16, 20 }
            },
            new Perk
            {
                id = 11,
                name = "Doble Filo",
                desc = "Activa. Hace un daño un 50/60/70/80/100% superior a un golpe normal, pero el usuario recibe un 50% del daño de un golpe normal.",
                lv = 0,
                reqLv = new int[5] { 2, 6, 10, 14, 18 }
            },
            new Perk
            {
                id = 12,
                name = "Golpe giro",
                desc = "Activa. Hace DMG/(2 - 0.2 * LVPERK) a todos los enemigos.",
                lv = 0,
                reqLv = new int[5] { 2, 6, 10, 14, 18 }
            },
            new Perk
            {
                id = 13,
                name = "Penetrador",
                desc = "Activa. Hace 1 + DMG/(2 - 0.2 * LVPERK) y rompe su armadura hasta después del siguiente ataque del ultimo en usar la habilidad",
                lv = 0,
                reqLv = new int[5] { 4, 8, 12, 16, 20 }
            },
            new Perk
            {
                id = 14,
                name = "Aturdir",
                desc = "Pasiva, incapacita a un enemigo durante 1 turno cuando la precision es 20 - LVPERK.",
                lv = 0,
                reqLv = new int[5] { 6, 10, 14, 18, 22 }
            }
        };

        public static Skill[] skills = new Skill[]
        {
            new Skill
            {
                id = 0,
                name = "Espadachin",
                desc = "Armas cortantes a una mano, no valen dos armas cada una en una mano",
            },
            new Skill
            {
                id = 1,
                name = "Kendo",
                desc = "Habilidad en el uso de sable japonés.",
            },
            new Skill
            {
                id = 2,
                name = "Jidai",
                desc = "Habilidad en el uso de sable de luz.",
            },
            new Skill
            {
                id = 3,
                name = "Medieval",
                desc = "Habilidad en el uso de espada a dos manos.",
            },
            new Skill
            {
                id = 4,
                name = "Maza",
                desc = "Habilidad en el uso de armas contundentes."
            },
            new Skill
            {
                id = 5,
                name = "Cuerpo",
                desc = "Habilidad para recibir golpes."
            },
            new Skill
            {
                id = 6,
                name = "Armadura Ligera",
                desc = "Habilidad en el uso de armaduras ligeras."
            },
            new Skill
            {
                id = 7,
                name = "Armadura Pesada",
                desc = "Habilidad en el uso de armaduras pesadas."
            },
            new Skill
            {
                id = 8,
                name = "Asesino",
                desc = "Habilidad en el uso de dagas."
            },
            new Skill
            {
                id = 9,
                name = "Poder Oculto",
                desc = "Habilidad en el uso de la habilidad oculta del jugador."
            },
            new Skill
            {
                id = 10,
                name = "Melee",
                desc = "Habilidad en el combate cuerpo a cuerpo con las manos."
            },
            new Skill
            {
                id = 11,
                name = "Lanzador",
                desc = "Habilidad en el lanzamiento de objetos."
            },
            new Skill
            {
                id = 12,
                name = "Tirador",
                desc = "Habilidad en el uso de armas a distancia."
            },
            new Skill
            {
                id = 13,
                name = "Nito-ryu",
                desc = "Habilidad en el uso de dos sables"
            }
        };
    }
}
