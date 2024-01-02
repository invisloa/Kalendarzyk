using Kalendarzyk.Models.EventModels;
using Kalendarzyk.ViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public class MicroTasksCCAdapterVM : BaseViewModel, IMicroTasksCC
	{
		private ICommand _currentCommand;
		public ICommand CurrentCommand
		{
			get => _currentCommand;
			set
			{
				_currentCommand = value;
				OnPropertyChanged();
			}
		}

		private MicroTaskModel _currentMicroTask;
		public MicroTaskModel CurrentMicroTask
		{
			get => _currentMicroTask;
			set
			{
				_currentMicroTask = value;
				OnPropertyChanged();
			}
		}

		private void UpdateCurrentCommand()
		{
			if (IsSelectedDeleteMode)
			{
				CurrentCommand = DeleteMicroTaskCommand;
			}
			else
			{
				CurrentCommand = SelectMicroTaskCommand;
			}
		}




		List<MicroTaskModel> _listToAddMicroTasks;
		public MicroTasksCCAdapterVM(List<MicroTaskModel> listToAddMicroTasks)
		{
			InitializeCommon();
			MicroTasksOC = new ObservableCollection<MicroTaskModel>(listToAddMicroTasks);
			AddMicroTaskEventCommand = new RelayCommand(AddMicroTaskEvent, CanAddMicroTaskEvent);
		}
		public RelayCommand ToggleDeleteModeCommand { get; set; }

		private void OnToggleDeleteMode()
		{
			IsSelectedDeleteMode = !IsSelectedDeleteMode;
			UpdateCurrentCommand();
		}
		private bool _isSelectedDeleteMode = false;
		public bool IsSelectedDeleteMode
		{
			get => _isSelectedDeleteMode;
			set
			{
				_isSelectedDeleteMode = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand<MicroTaskModel> DeleteMicroTaskCommand { get; set; }
		private void InitializeCommon()
		{
			SelectMicroTaskCommand = new RelayCommand<MicroTaskModel>(OnMicroTaskSelected);
			ToggleDeleteModeCommand = new RelayCommand(OnToggleDeleteMode);
			DeleteMicroTaskCommand = new RelayCommand<MicroTaskModel>(OnDeleteMicroTaskCommand);
			CurrentCommand = SelectMicroTaskCommand; // Set the default command
		}
		private string _microTaskToAddName;
		public string MicroTaskToAddName
		{
			get => _microTaskToAddName;
			set
			{
				if (_microTaskToAddName == value) { return; }
				_microTaskToAddName = value;
				OnPropertyChanged();
				AddMicroTaskEventCommand.RaiseCanExecuteChanged();
			}
		}
		public RelayCommand AddMicroTaskEventCommand { get; set; }

		private void OnDeleteMicroTaskCommand(MicroTaskModel microTask)
		{
			MicroTasksOC.Remove(microTask);
		}
		public void AddMicroTaskEvent()
		{
			MicroTasksOC.Add(new MicroTaskModel(MicroTaskToAddName));
			MicroTaskToAddName = "";
		}
		public bool CanAddMicroTaskEvent() => !string.IsNullOrWhiteSpace(MicroTaskToAddName);
		private bool _allMicroTasksCompleted;
		public bool AllMicroTasksCompleted
		{
			get => _allMicroTasksCompleted;
			set
			{
				if (_allMicroTasksCompleted == value)
				{
					return;
				}
				_allMicroTasksCompleted = value;
			}
		}
		private ObservableCollection<MicroTaskModel> _microTasksOC;
		public ObservableCollection<MicroTaskModel> MicroTasksOC
		{
			get => _microTasksOC;
			set
			{
				if (_microTasksOC != value)
				{
					_microTasksOC = value;
					OnPropertyChanged(nameof(MicroTasksOC));
				}
			}
		}
		public RelayCommand<MicroTaskModel> SelectMicroTaskCommand { get; set; }
		private void OnMicroTaskSelected(MicroTaskModel clickedMicrotask)
		{
			clickedMicrotask.IsMicroTaskCompleted = !clickedMicrotask.IsMicroTaskCompleted;
		}
	}
}
