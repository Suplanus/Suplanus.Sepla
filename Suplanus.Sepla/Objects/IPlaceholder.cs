namespace Suplanus.Sepla.Objects
{
	public interface IPlaceholder
	{
		string Description { get; set; }
		string Name { get; set; }
		object Value { get; set; }
	}
}