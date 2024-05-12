using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.EventOperations;

namespace Kalendarzyk.Views;

public partial class FavoritePage : ContentPage
{
	IEventRepository _eventRepository;
	FavoritePageViewModel favoritePageViewModel;
	private ISubEventTypeModel _subEventType;
	string _subTypeToLoadName;
	public FavoritePage()
	{
		_eventRepository = Factory.GetEventRepository();
		favoritePageViewModel = new FavoritePageViewModel(_eventRepository);
		BindingContext = favoritePageViewModel;
		_subTypeToLoadName = PreferencesManager.GetSubTypeQuickNoteName();
		InitializeComponent();
	}
	public FavoritePage(ISubEventTypeModel subEventType)
	{
		_subEventType = subEventType;
		_eventRepository = Factory.GetEventRepository();
		_subTypeToLoadName = subEventType.EventTypeName;
		favoritePageViewModel = new FavoritePageViewModel(_eventRepository, subEventType);
		BindingContext = favoritePageViewModel;
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		favoritePageViewModel.IsBusy = true;

		await LoadDataAsync();

		favoritePageViewModel.IsBusy = false;
	}

	private async Task LoadDataAsync()
	{
		//load data for the first time program runs
		if (_eventRepository.AllEventsList.Count == 0)
		{
			await _eventRepository.InitializeAsync();
			if(_subEventType == null)
			{
				favoritePageViewModel = new FavoritePageViewModel(_eventRepository);
				BindingContext = favoritePageViewModel;
			}


			else
			{
				favoritePageViewModel = new FavoritePageViewModel(_eventRepository, _subEventType);
				BindingContext = favoritePageViewModel;
			}
		}
		favoritePageViewModel.BindDataToShow(_subTypeToLoadName);

	}
}

