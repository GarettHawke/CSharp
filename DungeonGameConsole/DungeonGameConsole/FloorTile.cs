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
    public enum BackgroundType { Floor, Water, Wall, Entrance, Exit }

    public class FloorTile : IXmlSerializable
    {
        public BackgroundType Type { get; set; }
        public Item Item { get; set; }
        public bool IsHidden { get; set; }

        public FloorTile()
        {
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "FloorTile")
            {
                Type = (BackgroundType)Enum.Parse(typeof(BackgroundType), reader["Type"]);
                IsHidden = bool.Parse(reader["Hidden"]);
                if(reader.ReadToDescendant("Item"))
                {
                    Item = new Item();
                    Item.ReadXml(reader);
                }
                reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", Type.ToString());
            writer.WriteAttributeString("Hidden", IsHidden.ToString());
            if(Item != null)
            {
                writer.WriteStartElement("Item");
                Item.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
