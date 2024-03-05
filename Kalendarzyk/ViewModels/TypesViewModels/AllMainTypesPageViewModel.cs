using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using System.Collections.ObjectModel;
/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Services;
using System.Text;
After:
using System.Text;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Services;
using System.Text;
After:
using System.Text;
*/


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

		public AllMainTypesPageViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			AllMainEventTypesOC = new ObservableCollection<IMainEventType>(_eventRepository.AllMainEventTypesList);
			EditSelectedTypeCommand = new RelayCommand<IMainEventType>(EditSelectedType);

		}

		#endregion

		#region Public Methods

		#endregion

		private void EditSelectedType(IMainEventType subTypeToEdit)
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewMainTypePage(subTypeToEdit));
		}

	}
}