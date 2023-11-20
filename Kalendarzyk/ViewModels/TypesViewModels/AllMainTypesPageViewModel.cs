using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.ViewModels.TypesViewModels
{
	class AllMainTypesPageViewModel : BaseViewModel
	{
		#region Fields

		private ObservableCollection<IMainEventType> _allMainEventTypesOC;
		private IEventRepository _eventRepository;

		#endregion

		#region Properties


		public ObservableCollection<IMainEventType> AllMainEventTypesOC
		{
			get => _allMainEventTypesOC;
			set
			{
				_allMainEventTypesOC = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand<IMainEventType> EditSelectedTypeCommand { get; set; }

		#endregion

		#region Constructor
		// ctor

		public AllMainTypesPageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			AllMainEventTypesOC = new ObservableCollection<IMainEventType>(eventRepository.AllMainEventTypesList);
			EditSelectedTypeCommand = new RelayCommand<IMainEventType>(EditSelectedType);

		}

		#endregion

		#region Public Methods

		#endregion

		private void EditSelectedType(IMainEventType userTypeToEdit)
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewMainTypePage(_eventRepository, userTypeToEdit));
		}

	}
}