using System.Xml.Serialization;

namespace NParametrizer.Tests
{
	[XmlRoot("testSection")]
	public class CustomConfigSection
	{
		[XmlElement("testElement")]
		public string ConfigElement { get; set; }

		[XmlAttribute("number")]
		public int Number { get; set; }
	}
}
