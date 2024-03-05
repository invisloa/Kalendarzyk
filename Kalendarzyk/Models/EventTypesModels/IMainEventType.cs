namespace Kalendarzyk.Models.EventTypesModels
{
	public interface IMainEventType : IEquatable<object>
	{
		string Title { get; set; }
		IMainTypeVisualModel SelectedVisualElement { get; set; }
		new bool Equals(object other); // to check if the event type is already in the list
	}
}
