namespace Suplanus.Sepla.Objects
{
	public class GeneratablePageMacro
	{
		public GeneratablePageMacro(string filename, ILocationIdentifier locationIdentifier)
		{
			Filename = filename;
			LocationIdentifierIdentifier = (LocationIdentifierIdentifier) locationIdentifier;
		}

		public string Filename { get; set; }
		public LocationIdentifierIdentifier LocationIdentifierIdentifier { get; set; }
	}
}
