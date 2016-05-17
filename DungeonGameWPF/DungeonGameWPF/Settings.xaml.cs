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

namespace DungeonGameWPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public bool CharacterGender { get; private set; }
        public bool Sounds { get; private set; }

        public Settings()
        {
            InitializeComponent();
        }

        public Settings(bool gender, bool soundOn)
        {
            InitializeComponent();
            if (gender)
                femaleCh.IsChecked = true;
            else
                maleCh.IsChecked = true;
            if (soundOn)
                sound.IsChecked = true;
            else
                sound.IsChecked = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (maleCh.IsChecked.HasValue && !maleCh.IsChecked.Value)
                CharacterGender = false;
            else
                CharacterGender = true;

            if (sound.IsChecked.HasValue && sound.IsChecked.Value)
                Sounds = true;
            else
                Sounds = false;

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
