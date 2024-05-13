using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Kalendarzyk.Views.CustomControls;

public partial class ChangableIconCC : ContentView
{
	public static readonly BindableProperty IconFontTextProperty = BindableProperty.Create(
		nameof(IconFontText),
		typeof(string),
		typeof(ChangableIconCC),
		defaultValue: "default_icon");  // Set your default value here

	public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(
		nameof(IconTextColor),
		typeof(Color),
		typeof(ChangableIconCC),
		defaultValue: Colors.Blue);  // Set your default value here

	public static readonly BindableProperty IsSelectedCommandProperty = BindableProperty.Create(
		nameof(IsSelectedCommand),
		typeof(ICommand),
		typeof(ChangableIconCC),
		defaultValue: null);  // Set your default value here if needed

	public string IconFontText
	{
		get { return (string)GetValue(IconFontTextProperty); }
		set { SetValue(IconFontTextProperty, value); }
	}

	public Color IconTextColor
	{
		get { return (Color)GetValue(IconTextColorProperty); }
		set { SetValue(IconTextColorProperty, value); }
	}

	public ICommand IsSelectedCommand
	{
		get { return (ICommand)GetValue(IsSelectedCommandProperty); }
		set { SetValue(IsSelectedCommandProperty, value); }
	}

	public ChangableIconCC()
	{
		InitializeComponent();
	}
}
