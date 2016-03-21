using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DungeonGameConsole
{
    public enum ItemType { Diamond, Chest, GoldenKey, SilverKey, LockForGoldenKey, LockForSilverKey, LockBlock }

    public class Item : IXmlSerializable
    {
        public ItemType Type { get; set; }
        public bool? IsOpened { get; set; } //chest or lock
        public byte? LockBlockX { get; set; } //lock
        public byte? LockBlockY { get; set; } //lock
        public short? NumberOfDiamonds { get; set; } //chest
        public bool? HasSilverKey { get; set; } //chest
        public bool? HasGoldenKey { get; set; } //chest

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Item")
            {
                Type = (ItemType)Enum.Parse(typeof(ItemType), reader["Type"]);
                if (reader.GetAttribute("IsOpen") != null)
                    IsOpened = bool.Parse(reader["IsOpen"]);
                if (reader.GetAttribute("BlockX") != null)
                    LockBlockX = byte.Parse(reader["BlockX"]);
                if (reader.GetAttribute("BlockY") != null)
                    LockBlockY = byte.Parse(reader["BlockY"]);
                if (reader.GetAttribute("Diamonds") != null)
                    NumberOfDiamonds = short.Parse(reader["Diamonds"]);
                if (reader.GetAttribute("Golden") != null)
                    HasGoldenKey = bool.Parse(reader["Golden"]);
                if (reader.GetAttribute("Silver") != null)
                    HasSilverKey = bool.Parse(reader["Silver"]);
                reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", Type.ToString());
            if(IsOpened != null)
            {
                writer.WriteAttributeString("IsOpen", IsOpened.ToString());
                if(LockBlockX != null)
                {
                    writer.WriteAttributeString("BlockX", LockBlockX.ToString());
                    writer.WriteAttributeString("BlockY", LockBlockY.ToString());
                } else if(NumberOfDiamonds != null)
                {
                    writer.WriteAttributeString("Diamonds", NumberOfDiamonds.ToString());
                } else if(HasGoldenKey != null)
                {
                    writer.WriteAttributeString("Golden", HasGoldenKey.ToString());
                } else if(HasSilverKey != null)
                {
                    writer.WriteAttributeString("Silver", HasSilverKey.ToString());
                }
            }
        }
    }
}
