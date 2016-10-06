namespace Suplanus.Sepla.Objects
{
	/// <summary>
	/// Object to generate a project by PageMacros
	/// </summary>
	public class GeneratablePageMacro
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="filename">Filename of the PageMacro</param>
		/// <param name="locationIdentifier">Destination Loctions of Pages</param>
		public GeneratablePageMacro(string filename, ILocationIdentifier locationIdentifier)
		{
			Filename = filename;
			LocationIdentifierIdentifier = (LocationIdentifier) locationIdentifier;
		}

      /// <summary>
      /// Filename of the PageMacro
      /// </summary>
		public string Filename { get; set; }

      /// <summary>
      /// Destination Location of Pages
      /// </summary>
		public LocationIdentifier LocationIdentifierIdentifier { get; set; }
	}
}
