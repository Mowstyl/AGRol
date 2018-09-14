using AGRol.Models;
using AGRol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AGRol.Views
{
    /// <summary>
    /// Lógica de interacción para BattleChar.xaml
    /// </summary>
    public partial class BattleChar : Window
    {
        public List<Character> characters;
        public Dictionary<Character, CharPerk[]> perks;

        public BattleChar(List<Character> chars)
        {
            InitializeComponent();
            characters = chars;
            perks = new Dictionary<Character, CharPerk[]>();
            foreach (Character chara in characters)
            {
                initializePerks(chara);
            }
        }

        public void initializePerks(Character chara)
        {
            CharPerk[] aux = new CharPerk[MainMethods.perks.Length];
            for (int i = 0; i < MainMethods.perks.Length; i++)
            {
                aux[i] = new CharPerk { id = i, lv = 0 };
                while (aux[i].lv < MainMethods.perks[i].reqLv.Length && chara.stats.skills[i / 3].lv >= MainMethods.perks[i].reqLv[aux[i].lv])
                {
                    aux[i].lv++;
                }
            }
            perks[chara] = aux;
        }
    }
}
