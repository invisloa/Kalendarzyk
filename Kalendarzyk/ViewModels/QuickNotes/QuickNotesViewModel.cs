using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
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
		private ObservableCollection<IGeneralEventModel> _quickNotesOC;
		public QuickNotesViewModel(IEventRepository eventRepository)
		{
			_quickNotesOC = new ObservableCollection<IGeneralEventModel>(eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == "QNote"));
		}

	}
}
