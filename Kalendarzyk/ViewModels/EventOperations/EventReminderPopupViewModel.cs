//using Kalendarzyk.Models.EventModels;
//using Kalendarzyk.Services.DataOperations;
//using System.Windows.Input;

//namespace Kalendarzyk.ViewModels.EventOperations
//{
//    public class EventReminderPopupViewModel : EventOperationsBaseViewModel
//	{
//		private IGeneralEventModel _currentEvent;

//		public string Title => _currentEvent?.Title;
//		public string Description => _currentEvent?.Description;
//		IEventRepository _eventRepository;
//		public ICommand MarkAsCompleteCommand { get; }
//		public ICommand PostponeCommand { get; }
//		public ICommand EditEventCommand { get; }
//		public ICommand CloseCommand { get; }

//		public EventReminderPopupViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit) : base(eventRepository)
//		{
//			_currentEvent = eventToEdit;
//			MarkAsCompleteCommand = new RelayCommand(MarkAsComplete);
//			PostponeCommand = new RelayCommand(Postpone);
//			EditEventCommand = new RelayCommand(EditEvent);
//			CloseCommand = new RelayCommand(Close);
//		}

//		public void SetEvent()
//		{
//			// _currentEvent = eventModel;
//		}

//		private void MarkAsComplete()
//		{
//			// Implementation to mark the event as complete
//		}

//		private void Postpone()
//		{
//			// Implementation to postpone the event by 24 hours
//		}

//		private void EditEventAsync()
//		{
//			// Implementation to navigate to the event editing page
//		}

//		private void Close()
//		{
//			// Implementation to close the popup
//		}
//	}
//}
