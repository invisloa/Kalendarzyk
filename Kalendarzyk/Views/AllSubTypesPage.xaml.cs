using Kalendarzyk.ViewModels.TypesViewModels;

namespace Kalendarzyk.Views;

public partial class AllSubTypesPage : ContentPage
{
	public AllSubTypesPage()
	{
		BindingContext = new AllSubTypesPageViewModel();
		InitializeComponent();
	}
}