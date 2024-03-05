using Kalendarzyk.Models;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels
{
	internal class WelcomePageViewModel : BaseViewModel
	{
		private IMainEventTypesCCViewModel _mainEventTypesCCHelper;
		private IEventRepository _eventRepository;
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }
		public RelayCommand ButtonClickCommand { get; private set; }

		private string someIconString;
		public string SomeIconString
		{
			get
			{
				return someIconString;
			}
			set
			{
				someIconString = value;
				SomeIconToShow = Factory.CreateIMainTypeVisualElement(someIconString, Color.FromArgb("#FF0000"), Color.FromArgb("#FF0000"));
				OnPropertyChanged(nameof(SomeIconString));
			}
		}

		private IMainTypeVisualModel someIconToShow;
		public IMainTypeVisualModel SomeIconToShow
		{
			get
			{
				return someIconToShow;
			}
			set
			{
				someIconToShow = value;
				OnPropertyChanged(nameof(SomeIconToShow));
			}
		}

		public WelcomePageViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(_eventRepository.AllMainEventTypesList);
			ButtonClickCommand = new RelayCommand(OnButtonCommand);
		}

		private void OnButtonCommand()
		{
			var y = MainEventTypesVisualsOC[0].MainEventType.SelectedVisualElement.ElementName;
			SomeIconString = y;

		}



	}
}
