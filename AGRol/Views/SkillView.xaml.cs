using AGRol.Models;
using AGRol.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para SkillView.xaml
    /// </summary>
    public partial class SkillView : Window
    {
        //public Stats stats;
        public CreateChar vcwin;

        public SkillView(CreateChar vcwin)
        {
            InitializeComponent();
            this.vcwin = vcwin;
            lbSkills.ItemsSource = this.vcwin.chara.stats.skills;
        }
        public void refreshSkillList()
        {
            lbSkills.ItemsSource = null;
            lbSkills.ItemsSource = vcwin.chara.stats.skills;
        }

        private void lbSkills_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool aux = !(e.AddedItems.Count == 0);
            btAddXp.IsEnabled = aux;
            tbXP.IsEnabled = aux;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbXP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                ((TextBox)sender).Text = "0";
            }
            int aux;
            bool result = int.TryParse(((TextBox)sender).Text, out aux);
            if (aux < 0)
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void btAddXp_Click(object sender, RoutedEventArgs e)
        {
            int xp;
            bool result = int.TryParse(tbXP.Text, out xp);

            vcwin.chara.stats.addExp(xp, (MainMethods.SkillEnum)lbSkills.SelectedIndex);

            updateperks();

            vcwin.loadChar();
            refreshSkillList();
        }

        private void updateperks()
        {
            for (int i = 0; i < vcwin.chara.stats.perks.Length; i++)
            {
                int aux = 0;
                while (aux < MainMethods.perks[i].reqLv.Length && vcwin.chara.stats.skills[i/3].lv >= MainMethods.perks[i].reqLv[aux])
                {
                    aux++;
                }
                vcwin.chara.stats.perks[i].lv = aux;
            }
        }
    }
}
