using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views;

public partial class PreferencesPage : ContentPage
{
	public PreferencesPage()
	{

		var vm = new PreferencesViewModel();
		BindingContext = vm;
		InitializeComponent();
	}
}