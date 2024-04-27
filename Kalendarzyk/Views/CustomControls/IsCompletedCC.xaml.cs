using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls;

public partial class IsCompletedCC : ContentView
{
	public static readonly BindableProperty IconFontTextProperty = BindableProperty.Create(
		nameof(IconFontText),
		typeof(string),
		typeof(IsCompletedCC));

	public string IconFontText
	{
		get { return (string)GetValue(IconFontTextProperty); }
		set { SetValue(IconFontTextProperty, value); }
	}

	public static readonly BindableProperty IsSelectedCommandProperty = BindableProperty.Create(
		nameof(IsSelectedCommand),
		typeof(ICommand),
		typeof(IsCompletedCC));

	public ICommand IsSelectedCommand
	{
		get { return (ICommand)GetValue(IsSelectedCommandProperty); }
		set { SetValue(IsSelectedCommandProperty, value); }
	}

	public IsCompletedCC()
	{
		InitializeComponent();
	}
}
