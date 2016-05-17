namespace Suplanus.Sepla.Objects
{
	public interface IMacroPlaceholder
	{
		string Description { get; set; }
		string Name { get; set; }
		object Value { get; set; }
	}
}