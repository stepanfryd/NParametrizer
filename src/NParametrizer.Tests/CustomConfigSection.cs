using System.Xml.Serialization;

namespace NParametrizer.Tests
{
	/// <summary>
	/// Custom configuration section
	/// </summary>
	[XmlRoot("testSection")]
	public class CustomConfigSection
	{
		[XmlElement("testElement")]
		public string ConfigElement { get; set; }

		[XmlAttribute("number")]
		public int Number { get; set; }
	}
}
