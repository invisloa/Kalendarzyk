using Kalendarzyk.Models.EventModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions
{
	public interface IMicroTasksCC
	{
		string MicroTaskToAddName { get; set; }
		RelayCommand AddMicroTaskEventCommand { get; set; }
		ObservableCollection<MicroTask> MicroTasksOC { get; set; }
		RelayCommand<MicroTask> SelectMicroTaskCommand { get; set; }
	}
}
