using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	internal partial class ExtraOptionsSelectorHelperClass : ObservableObject
	{
		[ObservableProperty]
		private bool isModified;


		private bool _isQuickNoteMicroTasksType;

		public bool IsQuickNoteMicroTasksType
		{
			get => _isQuickNoteMicroTasksType;
			set
			{
				SetProperty(ref _isQuickNoteMicroTasksType, value);
				IsModified = true;
			}
		}
		private bool _isQuickNoteValueType;
		public bool IsQuickNoteValueType
		{
			get => _isQuickNoteValueType;
			set
			{
				SetProperty(ref _isQuickNoteValueType, value);
				IsModified = true;
			}
		}


		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand)),
				new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsQuickNoteValueTypeCommand)),
				new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnIsDateControlsSelectedCommand))
			};

		public ExtraOptionsSelectorHelperClass(IGeneralEventModel eventToEdit)
        {
			if (eventToEdit.QuantityAmount != null && eventToEdit.QuantityAmount.Value != 0)
			{
				OnIsMicroTasksSelectedCommand(QuickNotesButtonsSelectors[1]); // TODO refactor this
				DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == eventToEdit.QuantityAmount.Unit).First();
				DefaultMeasurementSelectorCCHelper.QuantityValue = eventToEdit.QuantityAmount.Value;
			}
			if (eventToEdit.MicroTasksList != null && eventToEdit.MicroTasksList.Count() > 0)
			{
				OnIsMicroTasksSelectedCommand(QuickNotesButtonsSelectors[0]); // TODO refactor this
				MicroTasksCCAdapter.MicroTasksOC = eventToEdit.MicroTasksList.ToObservableCollection();
			}
		}
		private void OnIsMicroTasksSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteMicroTasksType = !IsQuickNoteMicroTasksType;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsQuickNoteValueTypeCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteValueType = !IsQuickNoteValueType;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsDateControlsSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteDateSelected = !IsQuickNoteDateSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);
		}
	}
}
