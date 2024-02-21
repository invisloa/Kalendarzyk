using Kalendarzyk.Services;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views;

public partial class FirstRunPage : ContentPage
{
	public FirstRunPage()
	{
		BindingContext = new FirstRunViewModel();
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Application.Current.MainPage = new AppShell();
	}
}