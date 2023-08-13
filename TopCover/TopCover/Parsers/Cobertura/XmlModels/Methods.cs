using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

public class Methods
{
    [XmlElement("method")]
    public Method[] Items { get; set; } = null!;
}