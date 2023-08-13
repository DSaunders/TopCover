using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

public class Package
{
    [XmlAttribute("name")]
    public string Name { get; set; } = null!;

    [XmlElement("classes")]
    public Classes Classes { get; set; } = null!;
}