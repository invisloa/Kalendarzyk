using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.EventOperations;

namespace Kalendarzyk.Views;

public partial class ValueTypeCalculationsPage : ContentPage
{
	public ValueTypeCalculationsPage()
	{
		var viewModel = ServiceHelper.GetService<ValueTypeCalculationsViewModel>();

		BindingContext = viewModel;
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as ValueTypeCalculationsViewModel;
		viewModel.OnAppearing();
	}
}