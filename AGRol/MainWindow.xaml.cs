using AGRol.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json;
using AGRol.Views;

namespace AGRol
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Character> characters { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            characters = new List<Character>();
            lbChars.ItemsSource = characters;
        }

        private void createc_Click(object sender, RoutedEventArgs e)
        {
            CreateChar win = new CreateChar();
            win.mainw = this;
            win.Show();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\players.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, characters);
            }
        }

        private void btLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + @"\players.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    characters = (List<Character>)serializer.Deserialize(file, typeof(List<Character>));
                }

                refreshPlayerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error Loading!");
                MessageBox.Show(Directory.GetCurrentDirectory() + @"\players.json", "Error Loading!");
            }
        }

        public void refreshPlayerList()
        {
            lbChars.ItemsSource = null;
            lbChars.ItemsSource = characters;
        }

        private void lbChars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btViewC.IsEnabled = !(e.AddedItems.Count == 0);
        }

        private void btViewC_Click(object sender, RoutedEventArgs e)
        {
            //ViewChar vchar = new ViewChar((Character)lbChars.SelectedItem);
            CreateChar vchar = new CreateChar((Character)lbChars.SelectedItem);
            vchar.mainw = this;
            vchar.sindex = lbChars.SelectedIndex;
            vchar.Show();
        }
    }
}
