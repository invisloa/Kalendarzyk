using Kalendarzyk.Views.CustomControls.CCViewModels;

namespace Kalendarzyk.Views.CustomControls;

public partial class IsCompletedCC : ContentView
{
	public static readonly BindableProperty IconAdapterProperty = BindableProperty.Create(
			propertyName: "IconAdapter",
			returnType: typeof(ChangableFontsIconAdapter),
			declaringType: typeof(IsCompletedCC),
			defaultValue: null);

	public ChangableFontsIconAdapter IconAdapter
	{
		get => (ChangableFontsIconAdapter)GetValue(IconAdapterProperty);
		set => SetValue(IconAdapterProperty, value);
	}
	public IsCompletedCC()
	{
		InitializeComponent();
	}
}