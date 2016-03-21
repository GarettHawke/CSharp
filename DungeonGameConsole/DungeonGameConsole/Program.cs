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
            Level lvl = new Level(3, 6);
            lvl.LvlNumber = 0;
            lvl.Name = "Test";
            lvl.CharacterStartX = 0;
            lvl.CharacterStartY = 0;
            lvl.CharacterEndX = 3;
            lvl.CharacterEndY = 6;
            lvl.NumberOfDiamonds = 1;
            lvl.NumberOfDiamondsNeeded = 0;

            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 6; j++)
                {
                    lvl[i, j] = new FloorTile();
                    lvl[i, j].Type = BackgroundType.Floor;
                    lvl[i, j].IsHidden = false; 
                    if(i==0 && j==1)
                    {
                        lvl[i, j].Item = new Item();
                        lvl[i, j].Item.Type = ItemType.Diamond;
                    }
                }
            }

            /*System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(lvl.GetType());
            x.Serialize(Console.Out, lvl);*/
            lvl.ToXML("myFile.xml");
            Level n = Level.FromXml("myFile.xml");
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
