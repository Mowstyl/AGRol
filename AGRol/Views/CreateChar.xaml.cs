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
    /// Lógica de interacción para CreateChar.xaml
    /// </summary>
    public partial class CreateChar : Window
    {
        public Character chara;
        TextBox[] scoretb = new TextBox[6];
        TextBox[] adjusttb = new TextBox[6];
        TextBox[] bonustb = new TextBox[6];
        CheckBox[,] bonuschk = new CheckBox[2,6];
        int[] rscores = new int[6];
        int[] adjustments = new int[6];
        int baseac = 9;

        bool view = false;
        public int sindex = -1;

        public MainWindow mainw;

        public CreateChar()
        {
            InitializeComponent();

            chara = new Character();
            chara.resetCharacter();
            cbHiddenPower.ItemsSource = MainMethods.specs[0].hpower;

            initializeStructures();

            for (int i = 0; i < 6; i++)
            {
                rscores[i] = 9;
                chara.stats.scores[i] = 9;
                scoretb[i].Text = "9";
                adjustments[i] = 0;
                adjusttb[i].Text = "+0";
            }
            setSThrows(MainMethods.specs[0].sThrows);

            cbSpecies_SelectionChanged(null, null);

            tbExpLevelUp.Visibility = Visibility.Hidden;
            btSkills.Visibility = Visibility.Hidden;
        }

        public CreateChar(Character chara)
        {
            InitializeComponent();

            this.chara = chara;

            cbHiddenPower.IsEnabled = false;
            cbSpecies.SelectionChanged -= cbSpecies_SelectionChanged;
            tbHitPoints.IsEnabled = false;
            cbSpecies.IsEnabled = false;

            initializeStructures();

            tbHitPoints.Visibility = Visibility.Hidden;
            labelHP.Visibility = Visibility.Hidden;
            labelRHP.Content = "Total\nHit Points";
            chkLanA.IsEnabled = true;

            foreach (TextBox tb in bonustb)
            {
                tb.Visibility = Visibility.Hidden;
            }
            
            for (int i = 0; i < 6; i++)
            {
                bonuschk[0, i].Visibility = Visibility.Hidden;
                bonuschk[1, i].Visibility = Visibility.Hidden;
                bonuschk[0, i].Checked -= chkScore1_Checked;
                bonuschk[1, i].Checked -= chkScore2_Checked;
                bonuschk[0, i].Unchecked -= chkScore1_Unchecked;
                bonuschk[1, i].Unchecked -= chkScore2_Unchecked;
            }

            view = true;

            if (chara.stats.skills.Length < MainMethods.skills.Length)
            {
                CharSkill[] oldsk = chara.stats.skills;
                chara.stats.skills = new CharSkill[MainMethods.skills.Length];
                for (int i = 0; i < MainMethods.skills.Length; i++)
                {
                    if (i < oldsk.Length)
                    {
                        chara.stats.skills[i] = oldsk[i];
                    }
                    else
                    {
                        chara.stats.skills[i] = new CharSkill { id = i };
                    }
                }
            }

            if (chara.stats.perks.Length < MainMethods.perks.Length)
            {
                CharPerk[] oldpk = chara.stats.perks;
                chara.stats.perks = new CharPerk[MainMethods.perks.Length];
                for (int i = 0; i < MainMethods.perks.Length; i++)
                {
                    if (i < oldpk.Length)
                    {
                        chara.stats.perks[i] = oldpk[i];
                    }
                    else
                    {
                        chara.stats.perks[i] = new CharPerk { id = i };
                    }
                    int aux = 0;
                    while (aux < MainMethods.perks[i].reqLv.Length && chara.stats.skills[i / 3].lv >= MainMethods.perks[i].reqLv[aux])
                    {
                        aux++;
                    }
                    chara.stats.perks[i].lv = aux;
                }
            }

            loadChar();
        }

        private void initializeStructures()
        {
            scoretb[(int)MainMethods.StatEnum.STR] = tbStrength;
            scoretb[(int)MainMethods.StatEnum.INT] = tbIntelligence;
            scoretb[(int)MainMethods.StatEnum.WIS] = tbWisdom;
            scoretb[(int)MainMethods.StatEnum.DEX] = tbDexterity;
            scoretb[(int)MainMethods.StatEnum.CON] = tbConstitution;
            scoretb[(int)MainMethods.StatEnum.CHA] = tbCharisma;

            adjusttb[(int)MainMethods.StatEnum.STR] = tbStrAdj;
            adjusttb[(int)MainMethods.StatEnum.INT] = tbIntAdj;
            adjusttb[(int)MainMethods.StatEnum.WIS] = tbWisAdj;
            adjusttb[(int)MainMethods.StatEnum.DEX] = tbDexAdj;
            adjusttb[(int)MainMethods.StatEnum.CON] = tbConAdj;
            adjusttb[(int)MainMethods.StatEnum.CHA] = tbChaAdj;

            bonustb[(int)MainMethods.StatEnum.STR] = tbStrBonus;
            bonustb[(int)MainMethods.StatEnum.INT] = tbIntBonus;
            bonustb[(int)MainMethods.StatEnum.WIS] = tbWisBonus;
            bonustb[(int)MainMethods.StatEnum.DEX] = tbDexBonus;
            bonustb[(int)MainMethods.StatEnum.CON] = tbConBonus;
            bonustb[(int)MainMethods.StatEnum.CHA] = tbChaBonus;

            bonuschk[0, (int)MainMethods.StatEnum.STR] = chkStr1;
            bonuschk[1, (int)MainMethods.StatEnum.STR] = chkStr2;
            bonuschk[0, (int)MainMethods.StatEnum.INT] = chkInt1;
            bonuschk[1, (int)MainMethods.StatEnum.INT] = chkInt2;
            bonuschk[0, (int)MainMethods.StatEnum.WIS] = chkWis1;
            bonuschk[1, (int)MainMethods.StatEnum.WIS] = chkWis2;
            bonuschk[0, (int)MainMethods.StatEnum.DEX] = chkDex1;
            bonuschk[1, (int)MainMethods.StatEnum.DEX] = chkDex2;
            bonuschk[0, (int)MainMethods.StatEnum.CON] = chkCon1;
            bonuschk[1, (int)MainMethods.StatEnum.CON] = chkCon2;
            bonuschk[0, (int)MainMethods.StatEnum.CHA] = chkCha1;
            bonuschk[1, (int)MainMethods.StatEnum.CHA] = chkCha2;
        }

        public void loadChar()
        {
            for (int i = 0; i < 6; i++)
            {
                rscores[i] = chara.stats.scores[i] + chara.stats.rbonus[i];
                if (chara.stats.rbonus[i] > 0)
                {
                    rscores[i] += chara.stats.level;
                }
                else if (chara.species == (int)MainMethods.SpecEnum.Antroz || MainMethods.specs[chara.species].canDistr && MainMethods.specs[chara.species].adjust[i] > 0)
                {
                    rscores[i] += chara.stats.level / 2;
                }
                else
                {
                    int div = 3;
                    if (MainMethods.specs[chara.species].canDistr)
                    {
                        div = 4;
                    }
                    if (chara.stats.rbonus[i] < 0)
                    {
                        div = 5;
                    }
                    rscores[i] += chara.stats.level / div;
                }
                
                adjustments[i] = MainMethods.calcAdjustment(rscores[i]);

                scoretb[i].IsEnabled = false;
                scoretb[i].Text = rscores[i].ToString();
                string aux = string.Empty;
                if (adjustments[i] >= 0)
                {
                    aux = "+";
                }
                adjusttb[i].Text = aux + adjustments[i].ToString();
            }

            tbRHitPoints.Text = (MainMethods.specs[chara.species].hpcalc(chara.stats.hp, adjustments[4], chara.stats.level)).ToString();
            baseac = 9 - adjustments[(int)MainMethods.StatEnum.DEX];
            tbArmorClass.Text = baseac.ToString();
            tbRoll.Text = (19 - baseac).ToString();
            

            tbPlayerName.Text = chara.pname;
            tbDmName.Text = chara.dmname;
            tbCharacterName.Text = chara.cname;

            cbAlignment.SelectedIndex = (int)chara.alignment;
            cbHiddenPower.ItemsSource = MainMethods.specs[chara.species].hpower;
            cbHiddenPower.Text = chara.hpower;

            cbSpecies.SelectedIndex = (int)MainMethods.specs[chara.species].type;
            tbLevel.Text = chara.stats.level.ToString();
            tbExpLevelUp.Text = chara.stats.exp + "/" + (chara.stats.exp + chara.stats.nextlv);

            setLanguages(adjustments[(int)MainMethods.StatEnum.INT]);
            chkLanC.IsChecked = chara.languages["Antroz"];
            chkLanG.IsChecked = chara.languages["Gatusian"];
            chkLanR.IsChecked = chara.languages["Rokrok"];
            chkLanA.IsChecked = chara.languages["God"];

            tbQuirk.Text = chara.quirk;

            setSThrows(MainMethods.specs[chara.species].sThrows);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void setLanguages(int intAdj)
        {
            if (intAdj <= -3)
            {
                chkLanG.IsChecked = false;
                chkLanR.IsChecked = false;
                chkLanG.IsEnabled = false;
                chkLanR.IsEnabled = false;
                tbLanguages.Text = "Has trouble with speaking, cannot read or write";
                tbMaxLang.Text = "1";
            }
            else if (intAdj == -2)
            {
                chkLanG.IsChecked = false;
                chkLanR.IsChecked = false;
                chkLanG.IsEnabled = false;
                chkLanR.IsEnabled = false;
                tbLanguages.Text = "Cannot read or write Antroz";
                tbMaxLang.Text = "1";
            }
            else if (intAdj == -1)
            {
                chkLanG.IsChecked = false;
                chkLanR.IsChecked = false;
                chkLanG.IsEnabled = false;
                chkLanR.IsEnabled = false;
                tbLanguages.Text = "Can write simple Antroz words";
                tbMaxLang.Text = "1";
            }
            else
            {
                Species spec = getSpecies();
                int maxl = 2 + intAdj;
                if ((int)spec.type == 0 || (int)spec.type == 5)
                {
                    tbLanguages.Text = "Antroz";
                    chkLanG.IsChecked = false;
                    chkLanR.IsChecked = false;
                    chkLanG.IsEnabled = true;
                    chkLanR.IsEnabled = true;
                }
                else if ((int)spec.type < 3)
                {
                    chkLanG.IsChecked = true;
                    chkLanR.IsChecked = false;
                    chkLanG.IsEnabled = false;
                    chkLanR.IsEnabled = maxl > 2;
                    tbLanguages.Text = "Antroz, Gatusian";
                }
                else
                {
                    chkLanG.IsChecked = false;
                    chkLanR.IsChecked = true;
                    chkLanG.IsEnabled = maxl > 2;
                    chkLanR.IsEnabled = false;
                    tbLanguages.Text = "Antroz, Rokrok";
                }
                tbMaxLang.Text = maxl.ToString();
            }
        }

        private void setSThrows(int[] sThrows)
        {
            if (this.IsInitialized)
            {
                tbStPDR.Text = sThrows[0].ToString();
                tbStMW.Text = sThrows[1].ToString();
                tbStTTSP.Text = sThrows[2].ToString();
                tbStDB.Text = sThrows[3].ToString();
                tbStSMS.Text = sThrows[4].ToString();
            }
        }

        private Species getSpecies()
        {
            if (this.IsInitialized)
            {
                return MainMethods.specs[cbSpecies.SelectedIndex];
            }
            return null;
        }

        private void setScore(int value, bool check, int checknum, MainMethods.StatEnum stat)
        {
            if (!check)
            {
                rscores[(int)stat] = value + chara.stats.rbonus[(int)stat];
                chara.stats.scores[(int)stat] = value;
            }
            else
            {
                rscores[(int)stat] = chara.stats.scores[(int)stat] + checknum;
                chara.stats.rbonus[(int)stat] = checknum;
                bonustb[(int)stat].Text = "+" + checknum.ToString();
            }

            adjustments[(int)stat] = MainMethods.calcAdjustment(chara.stats.scores[(int)stat]);
            adjusttb[(int)stat].Text = adjustments[(int)stat].ToString();
            if (adjustments[(int)stat] >= 0)
            {
                adjusttb[(int)stat].Text = "+" + adjustments[(int)stat].ToString();
            }
            if (stat == MainMethods.StatEnum.INT)
            {
                chara.maxlanguages = 2 + adjustments[(int)stat];
                setLanguages(adjustments[(int)stat]);
            }
            if (stat == MainMethods.StatEnum.DEX)
            {
                tbArmorClass.Text = (baseac - adjustments[(int)stat]).ToString();
                tbRoll.Text = (19 - (baseac - adjustments[(int)stat])).ToString();
            }
            if (stat == MainMethods.StatEnum.CON)
            {
                tbRHitPoints.Text = getSpecies().hpcalc(chara.stats.hp, adjustments[4], 1).ToString();
            }
        }

        private void score_TextChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    TextBox source = (TextBox)sender;
                    MainMethods.StatEnum stat = getStatItem(source.Name);

                    int value = 9;
                    bool result = int.TryParse(source.Text, out value);
                    if (!result)
                    {
                        MessageBox.Show("A score must be a number!", "Error reading info");
                    }

                    if (value >= 3 && value <= 18)
                    {
                        setScore(value, false, 0, stat);
                    }
                    else
                    {
                        MessageBox.Show("Scores must be between 3 and 18", "Error reading info");
                    }
                }
            }
        }

        private void resetChecks()
        {
            if (this.IsInitialized)
            {
                for (int i = 0; i < 6; i++)
                {
                    bonuschk[1, i].IsChecked = false;
                    bonuschk[0, i].IsChecked = false;
                    bonuschk[1, i].IsEnabled = false;
                    bonuschk[0, i].IsEnabled = false;
                }
            }
        }

        private void cbSpecies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                resetChecks();

                Species spec = getSpecies();

                if (spec.canDistr)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        bonuschk[0, i].IsEnabled = spec.adjust[i] == 1;

                        chara.stats.rbonus[i] = 0;
                        bonustb[i].Text = "+0";
                        if (spec.adjust[i] == -1)
                        {
                            chara.stats.rbonus[i] = -1;
                            bonustb[i].Text = "-1";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        chara.stats.rbonus[i] = spec.adjust[i];
                        bonustb[i].Text = spec.adjust[i].ToString();
                        if (spec.adjust[i] >= 0)
                        {
                            bonustb[i].Text = "+" + bonustb[i].Text;
                        }
                    }
                }

                if (spec.forceAlign == 0)
                {
                    cbAlignment.SelectedIndex = 0;
                    cbAlignment.IsEnabled = false;
                    chara.alignment = (Character.Alignments)0;
                }
                else if (spec.forceAlign == 1)
                {
                    cbAlignment.SelectedIndex = 1;
                    cbAlignment.IsEnabled = false;
                    chara.alignment = (Character.Alignments)1;
                }
                else if (spec.forceAlign == 2)
                {
                    cbAlignment.SelectedIndex = 2;
                    cbAlignment.IsEnabled = false;
                    chara.alignment = (Character.Alignments)2;
                }
                else
                {
                    cbAlignment.IsEnabled = true;
                    chara.alignment = (Character.Alignments)cbAlignment.SelectedIndex;
                }

                cbHiddenPower.ItemsSource = null;
                cbHiddenPower.ItemsSource = spec.hpower;
                cbHiddenPower.SelectedIndex = 0;
                chara.hpower = cbHiddenPower.Text;
                chara.species = cbSpecies.SelectedIndex;

                for (int i = 0; i < 6; i++)
                {
                    setScore(chara.stats.scores[i], false, 0, (MainMethods.StatEnum)i);
                }
                setSThrows(spec.sThrows);

                tbRHitPoints.Text = getSpecies().hpcalc(chara.stats.hp, adjustments[4], 1).ToString();
            }
        }

        private int countChecks()
        {
            if (this.IsInitialized)
            {
                int counter = 0;
                CheckBox[] chks = new CheckBox[12] { chkStr1, chkStr2, chkInt1, chkInt2, chkWis1, chkWis2, chkDex1, chkDex2, chkCon1, chkCon2, chkCha1, chkCha2 };

                foreach (CheckBox chk in chks)
                {
                    if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                    {
                        counter++;
                    }
                }

                return counter;
            }
            return 0;
        }

        private void lockChecks()
        {
            if (this.IsInitialized)
            {
                CheckBox[] chks = new CheckBox[12] { chkStr1, chkInt1, chkWis1, chkDex1, chkCon1, chkCha1, chkStr2, chkInt2, chkWis2, chkDex2, chkCon2, chkCha2 };

                foreach (CheckBox chk in chks)
                {
                    if (!(chk.IsChecked.HasValue && chk.IsChecked.Value))
                    {
                        chk.IsEnabled = false;
                    }
                }
            }
        }

        private void unlockChecks()
        {
            if (this.IsInitialized)
            {
                CheckBox[] chks = new CheckBox[12] { chkStr1, chkInt1, chkWis1, chkDex1, chkCon1, chkCha1, chkStr2, chkInt2, chkWis2, chkDex2, chkCon2, chkCha2 };
                Species spec = getSpecies();

                for (int i = 0; i < 6; i++)
                {
                    chks[i].IsEnabled = spec.adjust[i] == 1;
                    chks[i + 6].IsEnabled = chks[i].IsChecked.HasValue && chks[i].IsChecked.Value;
                }
            }
        }

        private void btSkills_Click(object sender, RoutedEventArgs e)
        {
            SkillView skview = new SkillView(this);
            skview.Show();
        }

        private MainMethods.StatEnum getStatItem(string chkname)
        {
            MainMethods.StatEnum stat;

            if (chkname.ToUpper().Contains("STR"))
            {
                stat = MainMethods.StatEnum.STR;
            }
            else if (chkname.ToUpper().Contains("INT"))
            {
                stat = MainMethods.StatEnum.INT;
            }
            else if (chkname.ToUpper().Contains("WIS"))
            {
                stat = MainMethods.StatEnum.WIS;
            }
            else if (chkname.ToUpper().Contains("DEX"))
            {
                stat = MainMethods.StatEnum.DEX;
            }
            else if (chkname.ToUpper().Contains("CON"))
            {
                stat = MainMethods.StatEnum.CON;
            }
            else if (chkname.ToUpper().Contains("CHA"))
            {
                stat = MainMethods.StatEnum.CHA;
            }
            else
            {
                throw new ArgumentException("Item name must contain: STR, INT, WIS, DEX, CON or CHA (ignores uppercase/lowercase)");
            }

            return stat;
        }

        private void chkScore1_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox source = (CheckBox)sender;
            MainMethods.StatEnum stat = getStatItem(source.Name);

            if (countChecks() == 2)
            {
                lockChecks();
            }
            else if (countChecks() == 1)
            {
                bonuschk[1,(int)stat].IsEnabled = true;
            }

            setScore(0, true, 1, stat);
        }

        private void chkScore1_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox source = (CheckBox)sender;
            MainMethods.StatEnum stat = getStatItem(source.Name);

            setScore(0, true, 0, stat);

            unlockChecks();
        }

        private void chkScore2_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox source = (CheckBox)sender;
            MainMethods.StatEnum stat = getStatItem(source.Name);

            lockChecks();
            bonuschk[0,(int)stat].IsEnabled = false;

            setScore(0, true, 2, stat);
        }

        private void chkScore2_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox source = (CheckBox)sender;
            MainMethods.StatEnum stat = getStatItem(source.Name);

            setScore(0, true, 1, stat);

            unlockChecks();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            chara.quirk = tbQuirk.Text;
            if (view)
            {
                MainWindow.characters[sindex] = chara;
                mainw.refreshPlayerList();
                this.Close();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tbPlayerName.Text) || string.IsNullOrWhiteSpace(tbDmName.Text) || string.IsNullOrWhiteSpace(tbCharacterName.Text))
                {
                    MessageBox.Show("Player name, character name and DM name can't be empty", "Error reading info");
                }
                else
                {
                    chara.cname = tbCharacterName.Text;
                    chara.dmname = tbDmName.Text;
                    chara.pname = tbPlayerName.Text;

                    MainWindow.characters.Add(chara);
                    mainw.refreshPlayerList();
                    this.Close();
                }
            }
        }

        private void tbHitPoints_TextChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    TextBox source = (TextBox)sender;
                    Species spec = getSpecies();

                    int value = 1;
                    bool result = int.TryParse(source.Text, out value);
                    if (!result)
                    {
                        MessageBox.Show("Hit Points must be a number!", "Error reading info");
                    }

                    if (value >= spec.life[0] && value <= spec.life[1])
                    {
                        chara.stats.hp = value;
                        tbRHitPoints.Text = spec.hpcalc(value, adjustments[4], 1).ToString();
                    }
                    else
                    {
                        tbHitPoints.Text = chara.stats.hp.ToString();
                        MessageBox.Show("Hit Points must be between " + spec.life[0] + " and " + spec.life[1] + " for a " + cbSpecies.Text, "Error reading info");
                    }
                }
            }
        }

        private void chkLanG_Checked(object sender, RoutedEventArgs e)
        {
            tbLanguages.Text += ", Gatusian";
            chara.languages["Gatusian"] = true;
            int maxl = 2 + adjustments[1];
            if (maxl <= 2)
            {
                chkLanR.IsEnabled = false;
            }
        }

        private void chkLanG_Unchecked(object sender, RoutedEventArgs e)
        {
            string langs = "Antroz";
            chara.languages["Gatusian"] = false;
            if (chkLanR.IsChecked == true)
            {
                langs += ", Rokrok";
            }
            tbLanguages.Text = langs;

            int maxl = 2 + adjustments[1];
            if (maxl <= 2)
            {
                chkLanR.IsEnabled = true;
            }
        }

        private void chkLanR_Checked(object sender, RoutedEventArgs e)
        {
            tbLanguages.Text += ", Rokrok";
            chara.languages["Rokrok"] = true;
            int maxl = 2 + adjustments[1];
            if (maxl <= 2)
            {
                chkLanG.IsEnabled = false;
            }
        }

        private void chkLanR_Unchecked(object sender, RoutedEventArgs e)
        {
            string langs = "Antroz";
            chara.languages["Rokrok"] = false;
            if (chkLanG.IsChecked == true)
            {
                langs += ", Gatusian";
            }
            tbLanguages.Text = langs;

            int maxl = 2 + adjustments[1];
            if (maxl <= 2)
            {
                chkLanG.IsEnabled = true;
            }
        }

        private void cbAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                chara.alignment = (Character.Alignments)cbAlignment.SelectedIndex;
            }
        }
    }
}
