namespace Suplanus.Sepla.Objects
{
	public class GeneratablePageMacro
	{
		public GeneratablePageMacro(string filename, ILocationIdentifier locationIdentifier)
		{
			Filename = filename;
			LocationIdentifierIdentifier = (LocationIdentifier) locationIdentifier;
		}

		public string Filename { get; set; }
		public LocationIdentifier LocationIdentifierIdentifier { get; set; }
	}
}
