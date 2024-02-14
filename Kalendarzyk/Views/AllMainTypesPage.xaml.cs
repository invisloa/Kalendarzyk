using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.TypesViewModels;

namespace Kalendarzyk.Views;

public partial class AllMainTypesPage : ContentPage
{
	public AllMainTypesPage()
	{
		BindingContext = new AllMainTypesPageViewModel();

		InitializeComponent();
	}
}