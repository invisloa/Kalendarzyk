using Kalendarzyk.Models.EventModels;
using Kalendarzyk.ViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions;
using System.Collections.ObjectModel;
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

		private MicroTask _currentMicroTask;
		public MicroTask CurrentMicroTask
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




		public MicroTasksCCAdapterVM(IEnumerable<MicroTask> listToAddMicroTasks)
		{
				InitializeCommon();
				MicroTasksOC = new ObservableCollection<MicroTask>(listToAddMicroTasks);
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
		public RelayCommand<MicroTask> DeleteMicroTaskCommand { get; set; }
		private void InitializeCommon()
		{
			SelectMicroTaskCommand = new RelayCommand<MicroTask>(OnMicroTaskSelected);
			ToggleDeleteModeCommand = new RelayCommand(OnToggleDeleteMode);
			DeleteMicroTaskCommand = new RelayCommand<MicroTask>(OnDeleteMicroTaskCommand);
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

		private void OnDeleteMicroTaskCommand(MicroTask microTask)
		{
			MicroTasksOC.Remove(microTask);
		}
		public void AddMicroTaskEvent()
		{
			MicroTasksOC.Add(new MicroTask(MicroTaskToAddName));
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
		private ObservableCollection<MicroTask> _microTasksOC;
		public ObservableCollection<MicroTask> MicroTasksOC
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
		public RelayCommand<MicroTask> SelectMicroTaskCommand { get; set; }
		private void OnMicroTaskSelected(MicroTask clickedMicrotask)
		{
			clickedMicrotask.IsCompleted = !clickedMicrotask.IsCompleted;
		}
	}
}
