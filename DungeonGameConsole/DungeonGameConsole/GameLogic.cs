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
        private Character character;
        private bool soundOn;
        public bool SoundOn { get; set; }
        private Dictionary<string, short> score;

        public void loadSavedGame()
        {
            using(var fileStream = new FileStream("../../levelsFile.txt", FileMode.Open, FileAccess.Read))
            {
                using(var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null) {
                        levels.Add(line);
                    }
                }
            }

            using (var fileStream = new FileStream("../../settings.txt", FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    soundOn = bool.Parse(streamReader.ReadLine().Split('=')[1]);
                    currentLevel = Level.FromXml(streamReader.ReadLine().Split('=')[1]));
                }
            }
        }

        public void createNewGame()
        {
            File.Create("../../score.txt");
            if (score != null)
            {
                score.Clear();
                score = null;
            }
            currentLevel = Level.FromXml(levels.ElementAt(0));
            saveSettings();
        }

        private void saveSettings()
        {
            using (var fileStream = new FileStream("../../settings.txt", FileMode.Open, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine("soundOn=" + soundOn.ToString());
                    streamWriter.Write("currentLevel=" + currentLevel.LvlNumber);
                }
            }
        }

        public void setLevel(short lvl)
        {
            currentLevel = Level.FromXml(levels.ElementAt(lvl));
        }


        /*public bool playGame(ref Character c, ref Level lvl, GameActions act)
        {
            switch(act)
            {
                case GameActions.move:
                    break;
                case GameActions.open:
                    break;
                case GameActions.close:
                    break;
            }
            return false;
        }*/

        public byte getCharacterX()
        {
            return character.X;
        }

        public byte getCharacterY()
        {
            return character.Y;
        }

        public bool getHasSilverKey()
        {
            return character.HasSilverKey;
        }

        public bool getHasGoldenKey()
        {
            return character.HasGoldenKey;
        }

        public short getNumberOfDiamonds()
        {
            return character.NumberOfDiamondsCollected;
        }

        public bool moveCharacterBy(sbyte x, sbyte y)
        {
            FloorTile tile = currentLevel[(byte)(character.X + x), (byte)(character.Y + y)];
            switch (tile.Type)
            {
                case BackgroundType.Wall:
                case BackgroundType.Water:
                case BackgroundType.Entrance:
                    break;
                case BackgroundType.Exit:
                    return true;
                case BackgroundType.Floor:
                    if (tile.Item != null && tile.Item.Type == ItemType.Diamond)
                    {
                        tile.Item = null;
                        character.NumberOfDiamondsCollected++;
                    }
                    character.moveBy(x, y);
                    break;
            }
            return false;
        }

        public void openItem()
        {
            for (sbyte i = -1; i <= 1; i += 2)
            {
                for (sbyte j = -1; j <= 1; j += 2)
                {
                    Item item = currentLevel[character.X, character.Y].Item;
                    if (item.Type == ItemType.Chest)
                    {
                        item.IsOpened = true;
                        if (item.NumberOfDiamonds.HasValue)
                        {
                            character.NumberOfDiamondsCollected += item.NumberOfDiamonds.Value;
                            item.NumberOfDiamonds = null;

                        } else if (item.HasGoldenKey.HasValue)
                        {
                            character.HasGoldenKey = true;
                            item.HasGoldenKey = null;
                        } else if (item.HasSilverKey.HasValue)
                        {
                            character.HasSilverKey = true;
                            item.HasSilverKey = null;
                        }
                    } else if (item.Type == ItemType.LockForGoldenKey)
                    {
                        if (character.HasGoldenKey)
                        {
                            item.IsOpened = true;
                            currentLevel[item.LockBlockX.Value, item.LockBlockY.Value].Item = null;
                            character.HasGoldenKey = false;
                        }
                    } else if (item.Type == ItemType.LockForSilverKey)
                    {
                        if (character.HasSilverKey)
                        {
                            item.IsOpened = true;
                            currentLevel[item.LockBlockX.Value, item.LockBlockY.Value].Item = null;
                            character.HasSilverKey = false;
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
            using (var fileStream = new FileStream("../../score.txt", FileMode.Open, FileAccess.Read))
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
            //save score after lvl
            throw new NotImplementedException();
        }

        /*public void closeGame()
        {
            //close game
            //implement in
            //  -console in console context
            //  -graphical app in form logic
            throw new NotImplementedException();
        }*/
    }
}
