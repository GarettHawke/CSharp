using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGameConsole
{
    public class Character
    {
        public bool gender { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public short NumberOfDiamondsCollected { get; set; }
        public bool HasGoldenKey { get; set; }
        public bool HasSilverKey { get; set; }

        public Character()
        {
            reset();
        }

        public void moveAt(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public void moveBy(sbyte x, sbyte y)
        {
            X = (byte)(X + x);
            Y = (byte)(Y + y);
        }

        public void reset()
        {
            gender = false;
            X = 0;
            Y = 0;
            NumberOfDiamondsCollected = 0;
            HasGoldenKey = false;
            HasSilverKey = false;
        }
    }
}
