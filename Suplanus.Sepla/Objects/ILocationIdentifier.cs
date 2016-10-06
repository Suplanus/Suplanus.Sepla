namespace Suplanus.Sepla.Objects
{
	/// <summary>
	/// Interface of LocationIdentifier
	/// </summary>
	public interface ILocationIdentifier
	{
      /// <summary>
      /// FunctionalAssignment ==
      /// </summary>
		string FunctionAssignment { get; set; }		

      /// <summary>
      /// Location +
      /// </summary>
		string Location { get; set; }

      /// <summary>
      /// PlaceOfInstallation ++
      /// </summary>
		string PlaceOfInstallation { get; set; }

      /// <summary>
      /// Plant (Function) =
      /// </summary>
		string Plant { get; set; }

      /// <summary>
      /// Userdefinied #
      /// </summary>
		string UserDefinied { get; set; }

      /// <summary>
      /// DocType &amp;
      /// </summary>
      string DocType { get; set; }

      /// <summary>
      /// InstallationNumber (empty)
      /// </summary>
	   string InstallationNumber { get; set; }
   }
}