using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views;

public partial class PreferencesPage : ContentPage
{
	public PreferencesPage()
	{
		var eventRepository = ServiceHelper.GetService<IEventRepository>();

		var vm = new PreferencesViewModel(eventRepository);
		BindingContext = vm;
		InitializeComponent();
	}
}