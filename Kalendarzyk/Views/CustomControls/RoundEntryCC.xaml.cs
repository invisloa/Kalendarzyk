using Microsoft.Identity.Client;

namespace Kalendarzyk.Views.CustomControls;

public partial class RoundEntryCC : ContentView
{
	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(RoundEntryCC), default(string));
	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RoundEntryCC), default(string), BindingMode.TwoWay);
	public static readonly BindableProperty MyWidthRequestProperty = BindableProperty.Create(nameof(MyWidthRequest), typeof(int), typeof(RoundEntryCC), 150, BindingMode.TwoWay);

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}
	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}
	public int MyWidthRequest
	{
		get => (int)GetValue(MyWidthRequestProperty);
		set => SetValue(MyWidthRequestProperty, value);
	}

	public RoundEntryCC()
	{
		InitializeComponent();
	}
}