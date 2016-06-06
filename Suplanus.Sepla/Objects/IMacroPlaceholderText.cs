namespace Suplanus.Sepla.Objects
{
	public interface IMacroPlaceholderText
   {
		string Description { get; set; }
		string Name { get; set; }
		string Value { get; set; }
      bool IsActive { get; set; }
   }
}