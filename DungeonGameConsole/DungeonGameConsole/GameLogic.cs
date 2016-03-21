using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGameConsole
{
    class GameLogic
    {
        private List<String> levels;
        private Level currentLevel;
        private bool soundOn;

        public void initialize()
        {
            using(var fileStream = new FileStream("../../levelFile.txt", FileMode.Open, FileAccess.Read))
            {
                using(var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null) {
                        levels.Add(line);
                    }
                }
            }
            using(var)
        }

        public void createNewGame()
        {
            throw new NotImplementedException();
        }

        public void loadSavedGame()
        {
            throw new NotImplementedException();
        }

        public void setLevel()
        {
            throw new NotImplementedException();
        }

        public void playGame()
        {
            throw new NotImplementedException();
        }

        public void viewHighScore()
        {
            throw new NotImplementedException();
        }

        public void changeSettings()
        {
            throw new NotImplementedException();
        }

        public void saveGame()
        {
            throw new NotImplementedException();
        }

        public void closeGame()
        {
            throw new NotImplementedException();
        }
    }
}
