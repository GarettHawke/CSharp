using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DungeonGameConsole
{
    public class Level : IXmlSerializable
    {
        public short LvlNumber { get; set; }
        public String Name { get; set; }
        public byte CharacterStartX { get; set; }
        public byte CharacterStartY { get; set; }
        public byte CharacterEndX { get; set; }
        public byte CharacterEndY { get; set; }
        public short NumberOfDiamonds { get; set; }
        public short NumberOfDiamondsNeeded { get; set; }

        private FloorTile[][] floor;

        public Level() { }

        public Level(byte x, byte y)
        {
            floor = new FloorTile[x][];
            for(byte i = 0; i < x; i++)
            {
                floor[i] = new FloorTile[y];
            }
        }

        public FloorTile this[byte x, byte y]
        {
            get
            {
                return floor[x][y];
            }
            set
            {
                floor[x][y] = value;
            }
        }

        public byte sizeX()
        {
            return (byte)floor.Length;
        }

        public byte sizeY()
        {
            return (byte)floor[0].Length;
        }

        public static Level FromXml(String name)
        {
            XmlSerializer s = new XmlSerializer(typeof(Level));
            FileStream myFileStream = new FileStream(name, FileMode.Open);
            Level lvl = (Level)s.Deserialize(myFileStream);
            myFileStream.Close();
            return lvl;
        }

        public void ToXML(String name)
        {
            StreamWriter myWriter = new StreamWriter(name);
            XmlSerializer s = new XmlSerializer(typeof(Level));
            s.Serialize(myWriter, this);
            myWriter.Close();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Level")
            {
                LvlNumber = short.Parse(reader["Number"]);
                Name = reader["Name"];
                if (reader.ReadToDescendant("StartX"))
                    CharacterStartX = byte.Parse(reader.ReadElementContentAsString());
                if(reader.Name == "StartY")
                    CharacterStartY = byte.Parse(reader.ReadElementContentAsString());
                if (reader.Name == "EndX")
                    CharacterEndX = byte.Parse(reader.ReadElementContentAsString());
                if (reader.Name == "EndY")
                    CharacterEndY = byte.Parse(reader.ReadElementContentAsString());
                if (reader.Name == "InLevel")
                    NumberOfDiamonds = short.Parse(reader.ReadElementContentAsString());
                if (reader.Name == "Needed")
                    NumberOfDiamondsNeeded = short.Parse(reader.ReadElementContentAsString());
                if (reader.Name == "Floor")
                {
                    List<FloorTile[]> rows = new List<FloorTile[]>();
                    reader.ReadToDescendant("Row");
                    while (reader.LocalName == "Row")
                    {
                        List<FloorTile> floorTiles = new List<FloorTile>();
                        reader.ReadToDescendant("FloorTile");
                        while (reader.LocalName == "FloorTile")
                        {
                            FloorTile tile = new FloorTile();
                            tile.ReadXml(reader);
                            floorTiles.Add(tile);
                        }
                        rows.Add(floorTiles.ToArray());
                        reader.Read();
                    }
                    floor = rows.ToArray();
                }
                reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Number", LvlNumber.ToString());
            writer.WriteAttributeString("Name", Name);
            writer.WriteElementString("StartX", CharacterStartX.ToString());
            writer.WriteElementString("StartY", CharacterStartY.ToString());
            writer.WriteElementString("EndX", CharacterEndX.ToString());
            writer.WriteElementString("EndY", CharacterEndY.ToString());
            writer.WriteElementString("InLevel", NumberOfDiamonds.ToString());
            writer.WriteElementString("Needed", NumberOfDiamondsNeeded.ToString());
            writer.WriteStartElement("Floor");
            foreach(FloorTile[] row in floor)
            {
                writer.WriteStartElement("Row");
                foreach(FloorTile tile in row)
                {
                    writer.WriteStartElement("FloorTile");
                    tile.WriteXml(writer);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
