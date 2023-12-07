using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.QuickNotes;

namespace Kalendarzyk.Views.QuickNotes;

public partial class QuickNotesPage : ContentPage
{
	IEventRepository _eventRepository;
	public QuickNotesPage()
	{
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		InitializeComponent();
		BindingContext = new QuickNotesViewModel(_eventRepository);
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		await _eventRepository.InitializeAsync();
		BindingContext = new QuickNotesViewModel(_eventRepository);
	}
}