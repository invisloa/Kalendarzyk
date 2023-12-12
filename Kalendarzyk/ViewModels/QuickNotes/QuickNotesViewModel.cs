using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views.QuickNotes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.ViewModels.QuickNotes
{
	public partial class QuickNotesViewModel : ObservableObject
	{
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
		private ObservableCollection<IGeneralEventModel> _quickNotesOC;
		public QuickNotesViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			var quickNotesEvents = eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == "QNOTE");
			_quickNotesOC = new ObservableCollection<IGeneralEventModel>(quickNotesEvents);
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
			_quickNotesOC.Remove(quickNote);
		}
		private async Task OnEditSelectedQuickNoteCommand(IGeneralEventModel quickNote)
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddQuickNotesPage(_eventRepository, quickNote));
		}

		private async Task GoToAddQuickNote()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddQuickNotesPage());
		}

	}
}
