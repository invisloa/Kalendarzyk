using System.Collections;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls;

public partial class ColorButtonsSelectorCC : ContentView
{
	public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
	nameof(ItemsSource),
	typeof(IEnumerable),
	typeof(ColorButtonsSelectorCC),
	null);

	// Bindable property for Command
	public static readonly BindableProperty CommandProperty = BindableProperty.Create(
		nameof(Command),
		typeof(ICommand),
		typeof(ColorButtonsSelectorCC),
		null);

	public IEnumerable ItemsSource
	{
		get => (IEnumerable)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}
	public static readonly BindableProperty SelectColorCommandProperty = BindableProperty.Create(
		nameof(SelectColorCommand),
		typeof(ICommand),
		typeof(ColorButtonsSelectorCC),
		null,
		propertyChanged: OnSelectColorCommandChanged);

	public ICommand SelectColorCommand
	{
		get => (ICommand)GetValue(SelectColorCommandProperty);
		set => SetValue(SelectColorCommandProperty, value);
	}
	private static void OnSelectColorCommandChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (ColorButtonsSelectorCC)bindable;
		control.BindingContextChanged += (sender, e) =>
		{
			var bc = control.BindingContext;
			control.Content.BindingContext = bc;
		};
	}
	public ColorButtonsSelectorCC()
	{
		InitializeComponent();
	}


}