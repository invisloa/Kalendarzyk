using Kalendarzyk.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions
{
	public interface IMicroTasksCC
	{
		string MicroTaskToAddName { get; set; }
		RelayCommand AddMicroTaskEventCommand { get; set; }
		ObservableCollection<MicroTaskModel> MicroTasksOC { get; set; }
		RelayCommand<MicroTaskModel> SelectMicroTaskCommand { get; set; }
	}
}
