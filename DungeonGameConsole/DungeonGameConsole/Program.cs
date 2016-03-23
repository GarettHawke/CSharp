using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "**Dungeon Game**";
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;
            GameLogic game = new GameLogic();
            game.loadSavedGame();
            introScreen();
            do
            {
                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':
                        playGame(game);
                        introScreen();
                        break;
                    case '2':
                        setLevel(game.getLevelList());
                        game.setLevel(short.Parse(Console.ReadKey(true).KeyChar.ToString()));
                        playGame(game);
                        introScreen();
                        break;
                    case '3':
                        game.createNewGame();
                        introScreen();
                        break;
                    case '4':
                        viewScore(game.viewHighScore());
                        while (Console.ReadKey(true).KeyChar != 'b') ;
                        introScreen();
                        break;
                    case '5':
                        char pressedKey;
                        do
                        {
                            viewSettings(game.getSoundOn());
                            if ((pressedKey = Console.ReadKey(true).KeyChar) == 'm')
                            {
                                game.changeSoundOn();
                            }
                        } while (pressedKey == 'm' || (pressedKey != 'b' && (Console.ReadKey(true).KeyChar) != 'b'));
                        introScreen();
                        break;
                    case '6':
                        return;
                }
            } while (true);
        }

        private static void viewSettings(bool soundOn)
        {
            Console.Clear();
            Console.SetWindowSize(43, 9);
            Console.SetBufferSize(43, 9);
            Console.WriteLine("**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.WriteLine("<\t\t\t\t\t >");
            if (soundOn)
                Console.WriteLine("*\t\tSound: On (M)\t\t *");
            else
                Console.WriteLine("*\t\tSound: Off (M)\t\t *");
            Console.WriteLine("<\t\t\t\t\t >");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.WriteLine("<\t    Press \'b\' to return\t\t >");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.Write("**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**");
            Console.SetCursorPosition(0, 8);
        }

        private static void viewScore(Dictionary<string, short> dictionary)
        {
            Console.Clear();
            Console.SetWindowSize(43, 9);
            Console.SetBufferSize(43, 9);
            Console.WriteLine("**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**");
            Console.WriteLine("*\t\t\t\t\t *");
            if (dictionary.Count == 0)
            {
                Console.WriteLine("<\t\t\t\t\t >");
                Console.WriteLine("*\t\t No score\t\t *");
                Console.WriteLine("<\t\t\t\t\t >");
            }
            else
            {
                Console.WindowHeight = dictionary.Count + 6;
                Console.BufferHeight = dictionary.Count + 6;
                foreach (var item in dictionary)
                {
                    Console.WriteLine("<\t\t" + item.Key + " - " + item.Value);
                }
            }
            Console.WriteLine("*\t\t\t\t\t *");
            Console.WriteLine("<\t    Press \'b\' to return\t\t >");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.Write("**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**");
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
        }

        private static void setLevel(List<string> list)
        {
            Console.Clear();
            Console.SetWindowSize(43, list.Count + 4);
            Console.SetBufferSize(43, list.Count + 4);
            Console.WriteLine("**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**");
            Console.WriteLine("*\t\t\t\t\t *");
            foreach (var item in list)
            {
                Console.WriteLine("<\t" + item);
            }
            Console.WriteLine("*\t\t\t\t\t *");
            Console.Write("**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**");
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
        }

        private static void playGame(GameLogic game)
        {
            game.playGame();
            bool end = false;
            do
            {
                gameScreen(game);
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        end = game.moveCharacterBy(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        end = game.moveCharacterBy(0, 1);
                        break;
                    case ConsoleKey.DownArrow:
                        end = game.moveCharacterBy(1, 0);
                        break;
                    case ConsoleKey.LeftArrow:
                        end = game.moveCharacterBy(0, -1);
                        break;
                    case ConsoleKey.O:
                        game.openItem();
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.E:
                        return;
                }
            } while (!end);
            winScreen();
            Console.ReadLine();
        }

        private static void winScreen()
        {
            Console.Clear();
            Console.SetWindowSize(43, 5);
            Console.SetBufferSize(43, 5);

            Console.WriteLine("**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.WriteLine("<\t\t   WIN!!!\t\t >");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.Write("**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**");
            Console.SetCursorPosition(0, 4);
        }

        private static void gameScreen(GameLogic game)
        {
            Console.Clear();
            byte sizeX = game.getLevelSizeX();
            byte sizeY = game.getLevelSizeY();
            Console.SetWindowSize(sizeY + 10, sizeX + 9);
            Console.SetBufferSize(sizeY + 10, sizeX + 9);
            //Console.SetWindowSize(20, 20);
            //Console.SetBufferSize(20, 20);

            for (int i = 0; i < sizeY + 4; i++)
            {
                Console.Write('*');
            }
            Console.WriteLine();
            for (int i = 0; i < sizeY + 4; i++)
            {
                if (i == 0 || i == sizeY + 3)
                    Console.Write('*');
                else
                    Console.Write(' ');
            }
            Console.WriteLine();

            for (byte i = 0; i < sizeX; i++)
            {
                for (byte j = 0; j < sizeY; j++)
                {
                    if (j == 0)
                        Console.Write("* ");

                    FloorTile tile = game.getFloorTile(i, j);

                    if (tile.Type == BackgroundType.Floor)
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    else if (tile.Type == BackgroundType.Water)
                        Console.BackgroundColor = ConsoleColor.Blue;
                    else
                        Console.BackgroundColor = ConsoleColor.DarkRed;

                    if (tile.Type == BackgroundType.Entrance)
                        Console.Write('@');
                    else if (tile.Type == BackgroundType.Exit)
                        Console.Write('#');
                    else if (tile.Item != null)
                    {
                        if (tile.Item.Type == ItemType.Diamond)
                            Console.Write('O');
                        else if (tile.Item.Type == ItemType.Chest && !tile.Item.IsOpened.Value)
                            Console.Write('U');
                        else if (tile.Item.Type == ItemType.Chest && tile.Item.IsOpened.Value)
                            Console.Write('u');
                        else if (tile.Item.Type == ItemType.LockForGoldenKey && !tile.Item.IsOpened.Value)
                            Console.Write('G');
                        else if (tile.Item.Type == ItemType.LockForGoldenKey && tile.Item.IsOpened.Value)
                            Console.Write('g');
                        else if (tile.Item.Type == ItemType.LockForSilverKey && !tile.Item.IsOpened.Value)
                            Console.Write('S');
                        else if (tile.Item.Type == ItemType.LockForSilverKey && tile.Item.IsOpened.Value)
                            Console.Write('s');
                        else if (tile.Item.Type == ItemType.LockBlock)
                            Console.Write('=');
                    } else if (game.getCharacterX() == i && game.getCharacterY() == j)
                        Console.Write('*');
                    else
                        Console.Write(' ');
                    
                    Console.BackgroundColor = ConsoleColor.Black;

                    if (j == sizeY - 1)
                        Console.Write(" *");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < sizeY + 4; i++)
            {
                if (i == 0 || i == sizeY + 3)
                    Console.Write('*');
                else
                    Console.Write(' ');
            }
            Console.WriteLine();
            Console.WriteLine("S:" + game.getHasSilverKey().ToString() + " G:" + game.getHasGoldenKey().ToString());
            Console.WriteLine("D:" + game.getCharacterDiamonds().ToString() + " N:" + game.getNumberOfNeededDiamonds().ToString());

            Console.WriteLine();
            Console.WriteLine("Move by Arrows");
            Console.WriteLine("Exit - ESC, E");
            for (int i = 0; i < sizeY + 4; i++)
            {
                Console.Write('*');
            }
            Console.SetCursorPosition(0, sizeX + 3);
        }

        static private void introScreen()
        {
            Console.Clear();
            Console.SetWindowSize(43, 9);
            Console.SetBufferSize(43, 9);
            Console.WriteLine("**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**/\\**");
            Console.WriteLine("*\t\t 1. Play\t\t *");
            Console.WriteLine("<\t      2. Set Level\t\t >");
            Console.WriteLine("*\t   3. Create New Game\t\t *");
            Console.WriteLine("<\t      4. View Score\t\t >");
            Console.WriteLine("*\t       5. Settings\t\t *");
            Console.WriteLine("<\t\t6. Close\t\t >");
            Console.WriteLine("*\t\t\t\t\t *");
            Console.Write("**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**\\/**");
            Console.SetCursorPosition(0, 8);
        }
    }
}
