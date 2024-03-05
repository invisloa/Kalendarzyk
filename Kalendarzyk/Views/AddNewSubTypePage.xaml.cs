using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views;

public partial class AddNewSubTypePage : ContentPage
{
	public AddNewSubTypePage()
	{
		BindingContext = ServiceHelper.GetService<AddNewSubTypePageViewModel>();
		InitializeComponent();

	}
	public AddNewSubTypePage(ISubEventTypeModel userEventTypeModel)   // edit mode
	{
		BindingContext = new AddNewSubTypePageViewModel(userEventTypeModel);
		InitializeComponent();

	}

}
