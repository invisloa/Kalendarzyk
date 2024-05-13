using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;

/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Services;
After:
using Kalendarzyk.Views.QuickNotes;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Services;
After:
using Kalendarzyk.Views.QuickNotes;
*/
using Kalendarzyk.Views.QuickNotes;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels.QuickNotes
{
	public partial class QuickNotesViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool _isBusy = true;


		private IEventRepository _eventRepository;
		[ObservableProperty]
		private AsyncRelayCommand<IGeneralEventModel> _currentCommand;
		[ObservableProperty]
		private bool _isSelectedDeleteMode;

		[ObservableProperty]
		private AsyncRelayCommand<IGeneralEventModel> _deleteQuickNoteCommand;

		[ObservableProperty]
		private RelayCommand _toggleDeleteModeCommand;
		public AsyncRelayCommand GoToAddQuickNoteCommand => new AsyncRelayCommand(GoToAddQuickNote);

		[ObservableProperty]
		private AsyncRelayCommand<IGeneralEventModel> _editSelectedQuickNoteCommand;

		[ObservableProperty]
		private ObservableCollection<IGeneralEventModel> _quickNotesToShowOC;

		private string _searchBoxText;
		public string SearchBoxText
		{
			get => _searchBoxText;
			set
			{
				SetProperty(ref _searchBoxText, value);
				SearchQuickNotes();
			}
		}
		public void BindDataToShow()
		{
			var quickNotesEvents = _eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName());
			QuickNotesToShowOC = new ObservableCollection<IGeneralEventModel>(quickNotesEvents);
		}
		public QuickNotesViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			BindDataToShow();
			DeleteQuickNoteCommand = new AsyncRelayCommand<IGeneralEventModel>(OnDeleteQuickNoteCommand);
			EditSelectedQuickNoteCommand = new AsyncRelayCommand<IGeneralEventModel>(OnEditSelectedQuickNoteCommand);
			ToggleDeleteModeCommand = new RelayCommand(OnToggleDeleteMode);
			CurrentCommand = EditSelectedQuickNoteCommand; // Set the default command
		}
		private void OnToggleDeleteMode()
		{
			IsSelectedDeleteMode = !IsSelectedDeleteMode;
			UpdateCurrentCommand();
		}
		private void UpdateCurrentCommand()
		{
			if (IsSelectedDeleteMode)
			{
				CurrentCommand = DeleteQuickNoteCommand;
			}
			else
			{
				CurrentCommand = EditSelectedQuickNoteCommand;
			}
		}
		private async Task OnDeleteQuickNoteCommand(IGeneralEventModel quickNote)
		{
			await _eventRepository.DeleteFromEventsListAsync(quickNote);
			_quickNotesToShowOC.Remove(quickNote);
		}
		private async Task OnEditSelectedQuickNoteCommand(IGeneralEventModel quickNote)
		{
				await Application.Current.MainPage.Navigation.PushAsync(new AddQuickNotesPage(_eventRepository, quickNote));
		}

		private async Task GoToAddQuickNote()
		{
			await Application.Current.MainPage.Navigation.PushAsync(new AddQuickNotesPage());
		}
		private void SearchQuickNotes()
		{
			QuickNotesToShowOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName() && x.Title.Contains(SearchBoxText)));
		}
	}
}
