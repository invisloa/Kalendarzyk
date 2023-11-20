namespace Kalendarzyk.Models
{
	public interface IMainTypeVisualModel : IEquatable<object>
	{
		Color BackgroundColor { get; set; }
		string BackgroundColorString { get; set; }
		string ElementName { get; set; }
		Color TextColor { get; set; }
		string TextColorString { get; set; }
		bool Equals(object obj);
		int GetHashCode();
	}
}