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
        private Dictionary<string, short> score;

        public Character Character { get; private set; }
        public bool SoundOn { get; private set; }

        public GameLogic()
        {
            levels = new List<string>();
            Character = new Character();
        }

        public void loadSavedGame()
        {
            using(var fileStream = new FileStream("../../Conf/levelsFile.txt", FileMode.Open, FileAccess.Read))
            {
                using(var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null) {
                        levels.Add(line);
                    }
                }
            }

            using (var fileStream = new FileStream("../../Conf/settings.txt", FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    Character.gender = bool.Parse(streamReader.ReadLine().Split('=')[1]);
                    SoundOn = bool.Parse(streamReader.ReadLine().Split('=')[1]);
                    currentLevel = Level.FromXml(levels.ElementAt(int.Parse(streamReader.ReadLine().Split('=')[1])));
                }
            }
        }

        public List<string> getLevelList()
        {
            List<string> list = new List<string>();
            foreach(var lvl in levels)
            {
                StringBuilder s = new StringBuilder(lvl.Length);
                char c;
                int counter = lvl.Length - 5;
                do
                {
                    c = lvl.ElementAt(counter);
                    s.Append(c);
                    counter--;
                } while (c != '/' && c != '\\' && counter != -1);
                char[] charArray = s.Remove(s.Length - 1, 1).ToString().ToArray();
                Array.Reverse(charArray);
                list.Add(new string(charArray));
            }
            return list;
        }

        public void createNewGame()
        {
            File.Create("../../Conf/score.txt").Close();
            if (score != null)
            {
                score.Clear();
                score = null;
            }
            currentLevel = Level.FromXml(levels.ElementAt(0));
            saveSettings();
        }

        public void changeCharacterGender()
        {
            Character.gender = !Character.gender;
            saveSettings();
        }

        public void changeSoundOn()
        {
            SoundOn = !SoundOn;
            saveSettings();
        }

        private void saveSettings()
        {
            using (var fileStream = new FileStream("../../Conf/settings.txt", FileMode.Open, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine("character=" + Character.gender.ToString());
                    streamWriter.WriteLine("soundOn=" + SoundOn.ToString());
                    streamWriter.Write("currentLevel=" + currentLevel.LvlNumber);
                }
            }
        }

        public void setLevel(short lvl)
        {
            if(lvl < levels.Count)
                currentLevel = Level.FromXml(levels.ElementAt(lvl));
        }

        public void playGame()
        {
            Character.X = currentLevel.CharacterStartX;
            Character.Y = currentLevel.CharacterStartY;
            Character.NumberOfDiamondsCollected = 0;
            Character.HasGoldenKey = false;
            Character.HasSilverKey = false;
        }

        public bool moveCharacterBy(sbyte x, sbyte y)
        {
            FloorTile tile = currentLevel[(byte)(Character.X + x), (byte)(Character.Y + y)];
            switch (tile.Type)
            {
                case BackgroundType.Wall:
                case BackgroundType.Water:
                case BackgroundType.Entrance:
                    break;
                case BackgroundType.Exit:
                    if (Character.NumberOfDiamondsCollected >= currentLevel.NumberOfDiamondsNeeded)
                    {
                        saveGame();
                        setNextLevel();
                        return true;
                    }
                    break;
                case BackgroundType.Floor:
                    if (tile.Item != null && tile.Item.Type == ItemType.Diamond)
                    {
                        tile.Item = null;
                        Character.NumberOfDiamondsCollected++;
                        Character.moveBy(x, y);
                    } else if (tile.Item == null)
                    {
                        Character.moveBy(x, y);
                    }
                    break;
            }
            return false;
        }

        private void setNextLevel()
        {
            if (levels.Count == currentLevel.LvlNumber + 1)
            {
                currentLevel = Level.FromXml(levels.ElementAt(0));
            } else
            {
                currentLevel = Level.FromXml(levels.ElementAt(currentLevel.LvlNumber + 1));
            }
        }

        public byte getLevelSizeY()
        {
            return currentLevel.sizeY();
        }

        public byte getLevelSizeX()
        {
            return currentLevel.sizeX();
        }

        public void openItem()
        {
            for (sbyte i = -1; i <= 1; i += 1)
            {
                for (sbyte j = -1; j <= 1; j += 1)
                {
                    if(i == 0 ^ j == 0)
                    {
                        Item item = currentLevel[(byte)(Character.X + i), (byte)(Character.Y + j)].Item;
                        if (item != null)
                        {
                            if (item.Type == ItemType.Chest)
                            {
                                item.IsOpened = true;
                                if (item.NumberOfDiamonds.HasValue)
                                {
                                    Character.NumberOfDiamondsCollected += item.NumberOfDiamonds.Value;
                                    item.NumberOfDiamonds = null;

                                }
                                else if (item.HasGoldenKey.HasValue)
                                {
                                    Character.HasGoldenKey = true;
                                    item.HasGoldenKey = null;
                                }
                                else if (item.HasSilverKey.HasValue)
                                {
                                    Character.HasSilverKey = true;
                                    item.HasSilverKey = null;
                                }
                            }
                            else if (item.Type == ItemType.LockForGoldenKey)
                            {
                                if (Character.HasGoldenKey)
                                {
                                    item.IsOpened = true;
                                    currentLevel[item.LockBlockX.Value, item.LockBlockY.Value].Item = null;
                                    Character.HasGoldenKey = false;
                                }
                            }
                            else if (item.Type == ItemType.LockForSilverKey)
                            {
                                if (Character.HasSilverKey)
                                {
                                    item.IsOpened = true;
                                    currentLevel[item.LockBlockX.Value, item.LockBlockY.Value].Item = null;
                                    Character.HasSilverKey = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public FloorTile getFloorTile(byte x, byte y)
        {
            return currentLevel[x, y];
        }

        public Dictionary<string, short> viewHighScore()
        {
            score = new Dictionary<string, short>();
            using (var fileStream = new FileStream("../../Conf/score.txt", FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] levelScore = line.Split(' ');
                        score.Add(levelScore[0], short.Parse(levelScore[1]));
                    }
                }
            }
            return score;
        }

        private void saveGame()
        {
            Dictionary<string, short> s = new Dictionary<string, short>();
            using (var fileStream = new FileStream("../../Conf/score.txt", FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] levelScore = line.Split(' ');
                        s.Add(levelScore[0], short.Parse(levelScore[1]));
                    }
                }
            }

            string name = currentLevel.LvlNumber + "-" + currentLevel.Name;
            if (s.ContainsKey(name))
            {
                if (Character.NumberOfDiamondsCollected > s[name])
                    s[name] = Character.NumberOfDiamondsCollected;
            }
            else
            {
                s.Add(name, Character.NumberOfDiamondsCollected);
            }

            using (var fileStream = new FileStream("../../Conf/score.txt", FileMode.Open, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    foreach (var i in s)
                    {
                        streamWriter.WriteLine(i.Key + " " + i.Value);
                    }
                }
            }
        }
    }
}
