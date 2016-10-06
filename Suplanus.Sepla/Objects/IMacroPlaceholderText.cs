namespace Suplanus.Sepla.Objects
{
   /// <summary>
   /// Interface of MacroPlaceholderText
   /// </summary>
	public interface IMacroPlaceholderText
   {
      /// <summary>
      /// Description
      /// </summary>
		string Description { get; set; }

      /// <summary>
      /// Name
      /// </summary>
		string Name { get; set; }

      /// <summary>
      /// Container
      /// </summary>
      string Container { get; set; }

      /// <summary>
      /// Value
      /// </summary>
      string Value { get; set; }

      /// <summary>
      /// IsActive
      /// </summary>
      bool IsActive { get; set; }
   }
}