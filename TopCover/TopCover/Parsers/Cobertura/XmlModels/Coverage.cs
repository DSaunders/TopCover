using System.Xml.Serialization;

namespace TopCover.Parsers.Cobertura.XmlModels;

[XmlRoot(ElementName = "coverage")]
public class Coverage
{
    [XmlAttribute(AttributeName = "line-rate")]
    public decimal LineRate { get; set; }
    
    [XmlAttribute("branch-rate")]
    public decimal BranchRate { get; set; }
    
    [XmlAttribute("lines-valid")]
    public int TotalLines { get; set; }
    
    [XmlAttribute("lines-covered")]
    public int CoveredLines { get; set; }


    [XmlElement("packages")]
    public Packages Packages { get; set; } = null!;
}