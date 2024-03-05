using Kalendarzyk.Helpers;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.QuickNotes;

namespace Kalendarzyk.Views.QuickNotes;

public partial class QuickNotesPage : ContentPage
{
	IEventRepository _eventRepository;
	QuickNotesViewModel quickNotesViewModel;
	public QuickNotesPage()
	{
		_eventRepository = Factory.GetEventRepository();
		InitializeComponent();
		quickNotesViewModel = new QuickNotesViewModel();
		BindingContext = quickNotesViewModel;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		quickNotesViewModel.IsBusy = true;

		await LoadDataAsync();

		quickNotesViewModel.IsBusy = false;
	}

	private async Task LoadDataAsync()
	{
		//load data for the first time program runs
		if (_eventRepository.AllEventsList.Count == 0)
			await _eventRepository.InitializeAsync();
		quickNotesViewModel.BindDataToShow();
	}
}