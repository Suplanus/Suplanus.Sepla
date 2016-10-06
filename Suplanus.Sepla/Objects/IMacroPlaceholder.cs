namespace Suplanus.Sepla.Objects
{
	public interface IMacroPlaceholder
	{
		string Description { get; set; }
		string Name { get; set; }
      string Container { get; set; }
      object Value { get; set; }
      bool IsActive { get; set; }
   }
}