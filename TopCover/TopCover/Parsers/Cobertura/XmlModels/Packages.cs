using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

public class Packages
{
    [XmlElement("package")]
    public Package[] Items{ get; set; } = null!;
}