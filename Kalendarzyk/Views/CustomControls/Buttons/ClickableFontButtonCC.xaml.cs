namespace Kalendarzyk.Views.CustomControls.Buttons;

public partial class ClickableFontButtonCC : ContentView
{
	public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(
		nameof(IconTextColor), typeof(Color), typeof(ClickableFontButtonCC), Colors.Black,
		propertyChanged: OnIconTextColorPropertyChanged);
	public Color IconTextColor
	{
		get => (Color)GetValue(IconTextColorProperty);
		set => SetValue(IconTextProperty, value);
	}
	private static void OnIconTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ClickableFontButtonCC button)
		{
			// Raise a PropertyChanged event for IconTextColor
			button.OnPropertyChanged(nameof(IconTextColor));
		}
	}
	public static readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconTextProperty), typeof(string), typeof(ClickableFontButtonCC), "save", propertyChanged: OnIconTextFontPropertyChanged);
	public string IconText
	{
		get => (string)GetValue(IconTextProperty);
		set => SetValue(IconTextProperty, value);
	}
	private static void OnIconTextFontPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ClickableFontButtonCC button)
		{
			// Raise a PropertyChanged event for IconTextColor
			button.OnPropertyChanged(nameof(IconText));
		}
	}
	public static readonly BindableProperty SubmitCommandProperty = BindableProperty.Create(nameof(SubmitCommand), typeof(System.Windows.Input.ICommand), typeof(ClickableFontButtonCC), null);
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