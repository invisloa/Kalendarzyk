using Kalendarzyk.Helpers;
using Kalendarzyk.Models;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalendarzyk.Models.EventModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kalendarzyk.ViewModels
{
	public class AddNewMainTypePageViewModel : BaseViewModel
	{
		private readonly IEventRepository _eventRepository;
		private Dictionary<string, ObservableCollection<string>> _stringToOCMapper;
		private IMainEventType _currentMainType;
		private string _mainTypeName;
		private string _selectedVisualElementString;
		private Color _backgroundColor = Color.FromArgb("#fff");
		private Color _textColor = Color.FromArgb("#000");
		private bool _isEdit;
		private Dictionary<string, RelayCommand<SelectableButtonViewModel>> iconCommandsDictionary;
		private string lastSelectedIconType = "Top";
		private bool _isIconsTabSelected = true;
		private bool _isBgColorsTabSelected = false;
		private bool _isTextColorsTabSelected = false;
		public ObservableCollection<SelectableButtonViewModel> BgColorsButtonsOC { get; set; }
		public ObservableCollection<SelectableButtonViewModel> TextColorsButtonsOC { get; set; }
		public bool IsQuickNoteMainType
		{
			get => _currentMainType?.Title == "QNOTE";
		}
		public bool IsIconsTabSelected
		{
			get => _isIconsTabSelected;
			set
			{
				_isIconsTabSelected = value;
				OnPropertyChanged();
			}
		}
		public bool IsBgColorsTabSelected
		{
			get => _isBgColorsTabSelected;
			set
			{
				_isBgColorsTabSelected = value;
				OnPropertyChanged();
			}
		}
		public bool IsTextColorsTabSelected
		{
			get => _isTextColorsTabSelected;
			set
			{
				_isTextColorsTabSelected = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<SelectableButtonViewModel> MainButtonVisualsSelectors { get; set; }
		public ObservableCollection<SelectableButtonViewModel> IconsTabsOC { get; set; }

		public string SubmitMainTypeButtonText => _isEdit ? "SUBMIT CHANGES" : "ADD NEW MAIN TYPE";
		public string MainTypePlaceholderText => _isEdit ? $"TYPE NEW NAME FOR: {MainTypeName}" : "...NEW MAIN TYPE NAME...";


		#region Properties
		public string MainTypeName
		{
			get => _mainTypeName;
			set
			{
				_mainTypeName = value;
				OnPropertyChanged();
				AsyncSubmitMainTypeCommand.NotifyCanExecuteChanged();
			}
		}
		public bool IsEdit
		{
			get => _isEdit;
			set
			{
				_isEdit = value;
				OnPropertyChanged();
			}
		}
		public Color TextColor
		{
			get => _textColor;
			set
			{
				_textColor = value;
				OnPropertyChanged();
			}
		}
		public Color BackgroundColor
		{
			get => _backgroundColor;
			set
			{
				_backgroundColor = value;
				OnPropertyChanged();
			}
		}
		public string SelectedVisualElementString
		{
			get => _selectedVisualElementString;
			set
			{
				_selectedVisualElementString = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<string> IconsToShowStringsOC { get; set; }
		//public RelayCommand GoToAllMainTypesPageCommand { get; set; }
		public RelayCommand<string> ExactIconSelectedCommand { get; set; }
		public AsyncRelayCommand AsyncSubmitMainTypeCommand { get; set; }
		public AsyncRelayCommand DeleteAsyncSelectedMainEventTypeCommand { get; set; }
		public RelayCommand<SelectableButtonViewModel> BgColorsCommand { get; private set; }
		public RelayCommand<SelectableButtonViewModel> TextColorsCommand { get; private set; }
		#endregion


		#region Constructors
		//Constructor for create mode
		public AddNewMainTypePageViewModel()
		{
			IsEdit = false;
			_eventRepository = Factory.CreateNewEventRepository();
			InitializeCommon();
		}
		//Constructor for edit mode
		public AddNewMainTypePageViewModel(IMainEventType currentMainType)
		{
			IsEdit = true;
			_eventRepository = Factory.CreateNewEventRepository();
			InitializeCommon();
			_currentMainType = currentMainType;
			MainTypeName = currentMainType.Title;
			SelectedVisualElementString = currentMainType.SelectedVisualElement.ElementName;
			BackgroundColor = currentMainType.SelectedVisualElement.BackgroundColor;
			TextColor = currentMainType.SelectedVisualElement.TextColor;
			DeleteAsyncSelectedMainEventTypeCommand = new AsyncRelayCommand(OnDeleteMainTypeCommand, CanDeleteMainEventType);
		}
		#endregion

		#region public methods

		#endregion

		#region private methods
		private void InitializeCommon()
		{
			BgColorsButtonsOC = SelectableButtonHelper.GenerateColorPaletteButtons();
			TextColorsButtonsOC = SelectableButtonHelper.GenerateColorPaletteButtons();
			BgColorsCommand = new RelayCommand<SelectableButtonViewModel>(OnBgColorSeletctionCommand);
			TextColorsCommand = new RelayCommand<SelectableButtonViewModel>(OnTextColorSeletctionCommand);
			RefreshIconsToShowOC();
			InitializeCommands();
			InitializeSelectors();
		}
		private void InitializeIconsTabs()
		{
			IconsTabsOC = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Top", true, new RelayCommand(() => OnExactIconsTabCommand("Top"))),
				new SelectableButtonViewModel("Activities", false, new RelayCommand(() => OnExactIconsTabCommand("Activities"))),
				new SelectableButtonViewModel("IT", false, new RelayCommand(() => OnExactIconsTabCommand("IT"))),
				new SelectableButtonViewModel("Travel", false, new RelayCommand(() => OnExactIconsTabCommand("Travel"))),
				new SelectableButtonViewModel("Others", false, new RelayCommand(() => OnExactIconsTabCommand("Others"))),
			};
			RefreshIconsToShowOC();
			OnPropertyChanged(nameof(IconsTabsOC));
		}
		private void RefreshIconsToShowOC()
		{
			_stringToOCMapper = new Dictionary<string, ObservableCollection<string>>
			{
				{ "Top", IconsHelperClass.GetTopIcons() },
				{ "Activities", IconsHelperClass.GetActivitiesIcons() },
				{ "IT", IconsHelperClass.GetItIcons() },
				{ "Travel", IconsHelperClass.GetTravelIcons()},
				{ "Others", IconsHelperClass.GetOthersIcons() }
			};
		}
		private void InitializeCommands()
		{
			//GoToAllMainTypesPageCommand = new RelayCommand(OnGoToAllMainTypesPageCommand);
			AsyncSubmitMainTypeCommand = new AsyncRelayCommand(OnSubmitMainTypeCommand, CanExecuteSubmitMainTypeCommand);
			ExactIconSelectedCommand = new RelayCommand<string>(OnExactIconSelectedCommand);
		}

		private void InitializeSelectors()      // TODO CHANGE THIS TO DYNAMIC LIST !!!!!
		{
			SelectedVisualElementString = IconFont.Minor_crash;
			MainButtonVisualsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Icons", false, new RelayCommand<SelectableButtonViewModel>(OnShowIconsTabCommand)),
				new SelectableButtonViewModel("Background Colors", false, new RelayCommand<SelectableButtonViewModel>(OnShowBgColorsCommand)),
				new SelectableButtonViewModel("Text Colors", false, new RelayCommand<SelectableButtonViewModel>(OnShowTextColorsCommand)),
			};
			//InitializeIconsTabs();
		}

		private async Task OnSubmitMainTypeCommand()
		{
			var iconForMainEventType = Factory.CreateIMainTypeVisualElement(SelectedVisualElementString, BackgroundColor, TextColor);
			if (_isEdit)
			{
				if (await CanEditType())
				{
					var x = _eventRepository.AllMainEventTypesList.Single(x => x.Equals(_currentMainType));
					_currentMainType.Title = MainTypeName;
					_currentMainType.SelectedVisualElement = iconForMainEventType;
					MainTypeName = string.Empty;
					x = _currentMainType;
					await _eventRepository.UpdateMainEventTypeAsync(_currentMainType);
					await Shell.Current.GoToAsync("..");    // TODO CHANGE NOT WORKING!!!
				}
			}
			else
			{
				if (await CanAddNewType())
				{
					var newMainType = Factory.CreateNewMainEventType(MainTypeName, iconForMainEventType);
					MainTypeName = string.Empty;
					await _eventRepository.AddMainEventTypeAsync(newMainType);
					await Shell.Current.GoToAsync("..");    // TODO !!!!! CHANGE NOT WORKING!!!
				}
			}
		}
		private async Task<bool> CanAddNewType()
		{
			if (MainTypeName == "QNOTE")
			{
				await App.Current.MainPage.DisplayActionSheet($"Name is not allowed!!!", "OK", null);

				return false;
			}
			return true;
		}
		private async Task<bool> CanEditType()
		{
			if (_currentMainType.Title != "QNOTE" && MainTypeName == "QNOTE")
			{
				await App.Current.MainPage.DisplayActionSheet($"Name is not allowed!!!", "OK", null);
				return false;
			}
			return true;
		}
		//private void OnGoToAllMainTypesPageCommand()
		//{
		//	Shell.Current.GoToAsync("//AllMainTypesPage");

		//	//Application.Current.MainPage.Navigation.PushAsync(new AllMainTypesPage());
		//}
		private async Task OnDeleteMainTypeCommand()
		{
			var eventTypesInDb = _eventRepository.AllEventsList.Where(x => x.EventType.MainEventType.Equals(_currentMainType)); // to check
			if (eventTypesInDb.Any())
			{
				var action = await App.Current.MainPage.DisplayActionSheet("This main type is used...", "Cancel", null, "Delete all associated data", "\n", "Go to All SubTypes Page");
				switch (action)
				{
					case "Delete all associated data":
						// Perform the operation to delete all events of the event type.
						await DeleteMainEventType();
						await Shell.Current.GoToAsync("..");

						// TODO make a confirmation message
						break;
					case "Go to All SubTypes Page":
						// Redirect to the All Events Page.
						await Shell.Current.GoToAsync("AllSubTypesPage");   // TODO SELECT PROPPER MAINEVENTTYPE FOR THE PAGE
						break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;
			}
			else
			{
				await DeleteMainEventType();
				await Shell.Current.GoToAsync("..");
			}
		}
		private bool CanExecuteSubmitMainTypeCommand()
		{
			return !string.IsNullOrEmpty(MainTypeName);
		}

		private void OnExactIconsTabCommand(string iconType)
		{
			var lastSelectedButton = IconsTabsOC.Single(x => x.ButtonText == iconType);
			OnExactIconsTabClick(lastSelectedButton, _stringToOCMapper[iconType]);
		}
		private void OnExactIconsTabClick(SelectableButtonViewModel clickedButton, ObservableCollection<string> iconsToShowOC)
		{
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, IconsTabsOC);
			lastSelectedIconType = clickedButton.ButtonText;
			IconsToShowStringsOC = iconsToShowOC;
			OnPropertyChanged(nameof(IconsToShowStringsOC));
		}
		private async Task DeleteMainEventType()
		{
			// Perform the operation to delete all events of the event type.
			_eventRepository.AllEventsList.RemoveAll(x => x.EventType.MainEventType.Equals(_currentMainType));
			await _eventRepository.SaveEventsListAsync();
			_eventRepository.AllUserEventTypesList.RemoveAll(x => x.MainEventType.Equals(_currentMainType));
			await _eventRepository.SaveSubEventTypesListAsync();
			_eventRepository.AllMainEventTypesList.Remove(_currentMainType);
			await _eventRepository.SaveMainEventTypesListAsync();
		}
		private bool CanDeleteMainEventType()
		{
			return _currentMainType.Title != "QNOTE" ;
		}


		private void OnExactIconSelectedCommand(string visualStringSource)
		{
			SelectedVisualElementString = visualStringSource;
		}
		#endregion

		private void OnShowIconsTabCommand(SelectableButtonViewModel clickedButton)
		{
			SetAllSubTabsVisibilityOff();
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
			IsIconsTabSelected = true;
			InitializeIconsTabs();
			var buttonToSelect = IconsTabsOC.Single(x => x.ButtonText == lastSelectedIconType);
			OnExactIconsTabClick(buttonToSelect, _stringToOCMapper[lastSelectedIconType]);

		}
		private void ClearIconsTabs()
		{
			if (IconsTabsOC != null && IconsTabsOC.Any())
			{
				IconsTabsOC.Clear();
			}
			if (IconsToShowStringsOC != null && IconsToShowStringsOC.Any())
			{
				IconsToShowStringsOC.Clear();
			}
		}

		#region COLOR BUTTONS
		private void OnShowBgColorsCommand(SelectableButtonViewModel clickedButton)
		{
			SetAllSubTabsVisibilityOff();
			IsBgColorsTabSelected = true;
			ClearIconsTabs();
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
		}
		private void OnShowTextColorsCommand(SelectableButtonViewModel clickedButton)
		{
			SetAllSubTabsVisibilityOff();
			IsTextColorsTabSelected = true;
			ClearIconsTabs();
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
		}
		private void SetAllSubTabsVisibilityOff()
		{
			IsTextColorsTabSelected = false;
			IsBgColorsTabSelected = false;
			IsIconsTabSelected = false;
		}
		private void OnTextColorSeletctionCommand(SelectableButtonViewModel clickedButton)
		{
			TextColor = clickedButton.ButtonColor;
		}
		private void OnBgColorSeletctionCommand(SelectableButtonViewModel clickedButton)
		{
			BackgroundColor = clickedButton.ButtonColor;
		}
		#endregion
	}
}