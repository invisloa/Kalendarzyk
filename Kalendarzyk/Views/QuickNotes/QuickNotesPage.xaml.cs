using Kalendarzyk.Helpers;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.QuickNotes;

namespace Kalendarzyk.Views.QuickNotes;

public partial class QuickNotesPage : ContentPage
{
	IEventRepository _eventRepository;
	public QuickNotesPage()
	{
		_eventRepository = Factory.GetEventRepository();
		InitializeComponent();
		BindingContext = new QuickNotesViewModel();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		//load data for the first time program runs
		if (_eventRepository.AllEventsList.Count == 0)
		{
			await _eventRepository.InitializeAsync();
		}
		BindingContext = new QuickNotesViewModel();

	}

}