using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

public class Class
{
    [XmlAttribute("name")]
    public string Name { get; set; } = null!;
    
    [XmlAttribute(AttributeName = "line-rate")]
    public decimal LineRate { get; set; }
    
    [XmlAttribute("branch-rate")]
    public decimal BranchRate { get; set; }

    
    [XmlElement("methods")]
    public Methods Methods { get; set; } = null!;
}