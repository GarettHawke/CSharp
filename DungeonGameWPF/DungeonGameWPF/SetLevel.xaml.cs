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
    /// Interaction logic for SetLevel.xaml
    /// </summary>
    public partial class SetLevel : Window
    {
        public int SelectedLevel { get; set; }

        public SetLevel()
        {
            InitializeComponent();
        }

        public SetLevel(List<string> list)
        {
            InitializeComponent();

            foreach(var item in list)
            {
                levelsComboBox.Items.Add(item);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedLevel = levelsComboBox.SelectedIndex;

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
