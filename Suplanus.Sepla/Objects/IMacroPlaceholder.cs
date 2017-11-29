namespace Suplanus.Sepla.Objects
{
    /// <summary>
    /// Interface of MacroPlaceholder
    /// </summary>
    public interface IMacroPlaceholder
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
        object Value { get; set; }

        /// <summary>
        /// IsActive
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// IsExpanded
        /// </summary>
	    bool IsExpanded { get; set; }
    }
}