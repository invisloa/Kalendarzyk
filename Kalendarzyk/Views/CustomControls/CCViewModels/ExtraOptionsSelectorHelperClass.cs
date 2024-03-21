using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
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
		private MicroTasksCCAdapterVM _microTasksCCAdapter;
		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper;
		[ObservableProperty]
		private bool _isQuickNoteDateSelected;
		[ObservableProperty]
		private IsCompletedCCViewModel _isCompletedCCAdapter;
		private bool _isCompleted;
		[ObservableProperty]
		private bool _isQuickNoteMicroTasksType;
		[ObservableProperty]
		private bool _isQuickNoteValueType;
		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors;
		[ObservableProperty]
		private ISubEventTypeModel _subEventType;

		//ctor create mode
		public ExtraOptionsSelectorHelperClass()
		{
			InitializeCommon();


		}
		//ctor edit mode
		public ExtraOptionsSelectorHelperClass(IGeneralEventModel eventToEdit)
		{
			InitializeCommon();
			_isCompleted = eventToEdit.IsCompleted;

			_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTaskModel>());
			DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
			_isCompletedCCAdapter = Factory.CreateNewIsCompletedCCAdapter(_isCompleted);

			SetPropperValueType();

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
		private void InitializeCommon()
		{
			_quickNotesButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>		// TODO JO XXX REFACTOR THIS to be more modular
			{
				new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand)),
				new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsQuickNoteValueTypeCommand)),
				new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnIsDateControlsSelectedCommand))
			};
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

		private void SetPropperValueType()
		{
			_subEventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == event;
			var measurementUnitsForSelectedType = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(unit => unit.TypeOfMeasurementUnit == qNoteSubType.DefaultQuantityAmount.Unit); // TO CHECK!
			DefaultMeasurementSelectorCCHelper.QuantityAmount = qNoteSubType.DefaultQuantityAmount;
			DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC = new ObservableCollection<MeasurementUnitItem>(measurementUnitsForSelectedType);
			DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = measurementUnitsForSelectedType.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == qNoteSubType.DefaultQuantityAmount.Unit);
			OnPropertyChanged(nameof(DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC));

		}
	}
}
