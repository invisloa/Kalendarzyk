using CommunityToolkit.Maui.Core;

namespace Kalendarzyk.Views.CustomControls;

public partial class EventTypesCustomControl : ContentView
{
	//public static readonly BindableProperty ControlTextProperty = BindableProperty.Create(
	//	nameof(ControlText),
	//	typeof(string),
	//	typeof(EventTypesCustomControl),
	//	string.Empty);

	//public string ControlText
	//{
	//	get => (string)GetValue(ControlTextProperty);
	//	set => SetValue(ControlTextProperty, value);
	//}

	//public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
	////	nameof(IsExpanded),
	////	typeof(bool),
	////	typeof(EventTypesCustomControl),
	////	false,
	////	BindingMode.TwoWay,
	////	propertyChanged: OnIsExpandedChanged);


	//public bool IsExpanded
	//{
	//	get => (bool)GetValue(IsExpandedProperty);
	//	set => SetValue(IsExpandedProperty, value);
	//}
	//private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
	//{
	//	var control = (EventTypesCustomControl)bindable;
	//	if (control.MyExpander != null)
	//	{
	//		control.MyExpander.IsExpanded = (bool)newValue;
	//	}
	//}
	public EventTypesCustomControl()
	{
		InitializeComponent();
	}
	//private async void MyExpander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
	//{
	//	// Increment the current rotation by 180 degrees
	//	double newRotation = ArrowLabel.Rotation - 180;
	//	await ArrowLabel.RotateTo(newRotation, 350, Easing.CubicInOut);
	//}
}