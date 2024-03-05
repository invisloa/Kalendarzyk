using Kalendarzyk.Views.CustomControls.CCViewModels;

namespace Kalendarzyk.Views.CustomControls;

public partial class ColorButtonsSelectorCC : ContentView
{
	public static readonly BindableProperty ColorButtonsSelectorHelperProperty = BindableProperty.Create(
	nameof(ColorButtonsSelectorHelper),
	typeof(ColorButtonsSelectorViewModel),
	typeof(ColorButtonsSelectorCC),
	null);
	public ColorButtonsSelectorViewModel ColorButtonsSelectorHelper
	{
		get => (ColorButtonsSelectorViewModel)GetValue(ColorButtonsSelectorHelperProperty);
		set => SetValue(ColorButtonsSelectorHelperProperty, value);
	}
	public ColorButtonsSelectorCC()
	{
		InitializeComponent();
	}


}