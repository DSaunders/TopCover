using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

public class Method
{
    [XmlAttribute("name")]
    public string Name { get; set; } = null!;
}