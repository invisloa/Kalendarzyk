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
		_eventRepository = Factory.CreateNewEventRepository();
		InitializeComponent();
		BindingContext = new QuickNotesViewModel();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		//<
		//TODO JO !!!!!!!!! MOVE THIS TO SOME LOADING STARTING PAGE!!!!!!!!!!!!
		await _eventRepository.InitializeAsync();
		await PreferencesViewModel.AddQuickNotesTypes();
		//TODO JO !!!!!!!!! MOVE THIS TO SOME LOADING STARTING PAGE!!!!!!!!!!!!
		//>


		BindingContext = new QuickNotesViewModel();
	}

}