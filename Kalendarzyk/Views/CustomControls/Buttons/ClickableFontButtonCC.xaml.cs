using Microsoft.Identity.Client;

namespace Kalendarzyk.Views.CustomControls.Buttons;

public partial class ClickableFontButtonCC : ContentView
{
	public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(nameof(IconTextColorProperty), typeof(Color), typeof(ClickableFontButtonCC), Colors.Black);
	public Color IconTextColor
	{
		get => (Color)GetValue(IconTextColorProperty);
		set => SetValue(IconTextProperty, value);
	}

	public static readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconTextProperty), typeof(string), typeof(ClickableFontButtonCC), "save");
	public string IconText
	{
		get => (string)GetValue(IconTextProperty);
		set => SetValue(IconTextProperty, value);
	}

	public static readonly BindableProperty SubmitCommandProperty = BindableProperty.Create(nameof(Command), typeof(System.Windows.Input.ICommand), typeof(ClickableFontButtonCC), null);
	public System.Windows.Input.ICommand SubmitCommand
	{
		get => (System.Windows.Input.ICommand)GetValue(SubmitCommandProperty);
		set => SetValue(SubmitCommandProperty, value);
	}
	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSizeProperty), typeof(int), typeof(ClickableFontButtonCC), 32);
	public int FontSize
	{
		get => (int)GetValue(FontSizeProperty);
		set => SetValue(FontSizeProperty, value);
	}
	public ClickableFontButtonCC()
	{

		InitializeComponent();
	}
}