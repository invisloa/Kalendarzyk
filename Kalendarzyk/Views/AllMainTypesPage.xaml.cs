using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.TypesViewModels;

namespace Kalendarzyk.Views;

public partial class AllMainTypesPage : ContentPage
{
	IEventRepository _eventRepository;
	public AllMainTypesPage()
	{
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		BindingContext = new AllMainTypesPageViewModel(_eventRepository);

		InitializeComponent();
	}
}