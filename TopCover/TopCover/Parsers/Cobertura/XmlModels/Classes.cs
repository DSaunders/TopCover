using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

public class Classes
{
    [XmlElement("class")]
    public Class[] Items { get; set; } = null!;
}