using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces
{

	/// <summary>
	/// When using this interface consider using MainEventTypesCCHelper class
	/// MainEventTypesCCHelper implements this interface and helps to set the logic for control operations 
	/// </summary>
	public interface IMainEventTypesCCViewModel
	{
		public IMainEventType SelectedMainEventType { get; set; }
		ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get; set; }
		RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; }
		public event Action<IMainEventType> MainEventTypeChanged;
	}
}
