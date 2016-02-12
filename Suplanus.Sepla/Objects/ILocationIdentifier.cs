namespace Suplanus.Sepla.Objects
{
	public interface ILocationIdentifier
	{
		string FunctionAssignment { get; set; }		
		string Location { get; set; }
		string PlaceOfInstallation { get; set; }
		string Plant { get; set; }
		string UserDefinied { get; set; }
	}
}