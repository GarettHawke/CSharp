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
using System.Windows.Navigation;
using System.Windows.Shapes;

using DungeonGameConsole;

namespace DungeonGameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameLogic logic;
        private bool isPlaying;

        public MainWindow()
        {
            InitializeComponent();
            logic = new GameLogic();
            logic.loadSavedGame();
        }

        private void newGameMI_Click(object sender, RoutedEventArgs e)
        {
            logic.createNewGame();
            MessageBox.Show("Created New Game!\nPress \'Play\' to start!", "New Game");
        }

        private void updateGame()
        {
            //vykreslovanie canvas
            throw new NotImplementedException();
        }

        private void playGameMI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void setLevelMI_Click(object sender, RoutedEventArgs e)
        {
            SetLevel set = new SetLevel(logic.getLevelList());
            set.ShowDialog();
            if(set.DialogResult.HasValue && set.DialogResult.Value)
            {
                if(set.SelectedLevel >= 0)
                {
                    logic.setLevel((short)set.SelectedLevel);
                }
                else
                {
                    MessageBox.Show("No level selected!");
                }
            }
        }

        private void scoreMI_Click(object sender, RoutedEventArgs e)
        {
            var score = logic.viewHighScore();
            if (score.Count == 0)
                MessageBox.Show("No score!", "Score");
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach(var item in score)
                {
                    sb.Append(item.Key + " - " + item.Value + "\n");
                }
                MessageBox.Show(sb.ToString(), "Score");
            }
        }

        private void settingsMI_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings(logic.Character.gender, logic.SoundOn);
            set.ShowDialog();
            if(set.DialogResult.HasValue && set.DialogResult.Value)
            {
                if (logic.SoundOn != set.Sounds)
                    logic.changeSoundOn();
                if (logic.Character.gender != set.CharacterGender)
                    logic.changeCharacterGender();
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void aboutMI_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dungeon Game 2016\nPeter Vašek", "DungeonGame");
        }

        private void createLevelMI_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented!", "Info");
        }

        private void editLevelMI_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented!", "Info");
        }

        private void controlMI_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Move by Arrow Keys");
            sb.AppendLine("Open items by \'o\'");
            sb.Append("Exit by (ESC)");
            MessageBox.Show(sb.ToString(), "Control");
        }
    }
}
