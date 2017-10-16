namespace Suplanus.Sepla.Objects
{
   /// <summary>
   /// MacroPlaceholder
   /// </summary>
   public class MacroPlaceholder : IMacroPlaceholder
   {
      /// <summary>
      /// Description
      /// </summary>
		public string Description { get; set; }

      /// <summary>
      /// Name
      /// </summary>
		public string Name { get; set; }

      /// <summary>
      /// Container
      /// </summary>
      public string Container { get; set; }

      /// <summary>
      /// Value
      /// </summary>
      public object Value { get; set; }

      /// <summary>
      /// IsActive
      /// </summary>
      public bool IsActive { get; set; }
   }

}
