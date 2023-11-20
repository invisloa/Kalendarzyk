using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls;

public partial class ColorButtonsSelectorCC : ContentView
{
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

	public ColorButtonsSelectorCC()
	{
		InitializeComponent();
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
}