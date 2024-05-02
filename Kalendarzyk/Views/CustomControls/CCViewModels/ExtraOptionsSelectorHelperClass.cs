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

	// TODO HERE 27.03.24 WHEN SWITCHING SUB TYPE IN EVENTS ADDING THE CONTROLS cONTROLS ARE NOT REFRESHED Accordingly
	public partial class ExtraOptionsSelectorHelperClass : ObservableObject
	{
		private bool _isEventMicroTasksType;
		public bool IsEventMicroTasksType
		{
			get => _isEventMicroTasksType;
			set
			{
				SetProperty(ref _isEventMicroTasksType, value);
				if (!value)
				{
					IsMicroTasksBtnSelected = false;
				}
			}
		}

		private bool _isEventValueType;
		public bool IsEventValueType
		{
			get => _isEventValueType;
			set
			{
				SetProperty(ref _isEventValueType, value);
				if (!value)
				{
					IsValueBtnSelected = false;
				}
			}
		}
		private bool _isMicroTasksBtnSelected;
		public bool IsMicroTasksBtnSelected
		{
			get 
				{ if (IsEventMicroTasksType)
					{
						return _isMicroTasksBtnSelected;
					}
				else
					{
						return false;
					}
				}
			set => SetProperty(ref _isMicroTasksBtnSelected, value);
		}
		private bool _isValueBtnSelected;
		public bool IsValueBtnSelected
		{
			get
			{
				if (IsEventValueType)
				{
					return _isValueBtnSelected;
				}
				else
				{
					return false;
				}
			}
			set => SetProperty(ref _isValueBtnSelected, value);
		}
		private IGeneralEventModel _eventToEdit;

		[ObservableProperty]
		private MicroTasksCCAdapterVM _microTasksCCAdapter;
		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
		[ObservableProperty]
		private bool _isDateBtnSelected;
		[ObservableProperty]
		private ChangableFontsIconAdapter _changableFontsIconCC;
		private bool _isCompleted;
		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _extraOptionsButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>();
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
			_eventToEdit = eventToEdit;
			_subEventType= eventToEdit.EventType;

            InitializeCommon();

			_changableFontsIconCC = Factory.CreateNewChangableFontsIconAdapter(false, "check_box", "check_box_outline_blank");

			if (_eventToEdit.EventType.IsValueType)
			{
				IsEventValueType = true;
				DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
				DefaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
				if (eventToEdit.EventType.QuantityAmount != null && eventToEdit.EventType.QuantityAmount.Value != 0)
				{
					OnIsEventValueTypeCommand(ExtraOptionsButtonsSelectors[1]); // TODO refactor this
					DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == _eventToEdit.EventType.QuantityAmount.Unit).First();
					DefaultMeasurementSelectorCCHelper.QuantityValue = _eventToEdit.EventType.QuantityAmount.Value;
				}
				DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(_subEventType);
			}

			if (_eventToEdit.EventType.IsMicroTaskType)
			{
				IsEventMicroTasksType = true;
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTaskModel>());

				if (_eventToEdit.EventType.MicroTasksList != null && _eventToEdit.EventType.MicroTasksList.Count() > 0)
				{
					OnIsMicroTasksSelectedCommand(ExtraOptionsButtonsSelectors[0]); // TODO refactor this
					MicroTasksCCAdapter.MicroTasksOC = _eventToEdit.EventType.MicroTasksList.ToObservableCollection();
				}
			}
			SetPropperValueType();

		}
		private void InitializeCommon() // TODO JO XXX REFACTOR THIS to be more modular
		{
			if (ExtraOptionsButtonsSelectors.Count == 0)
			{
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand), isEnabled: _subEventType?.IsMicroTaskType == true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsEventValueTypeCommand), isEnabled: _subEventType?.IsValueType == true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnIsDateControlsSelectedCommand)));
			}
			else
			{
                ExtraOptionsButtonsSelectors[0].IsEnabled = _subEventType?.IsMicroTaskType ?? true;
                ExtraOptionsButtonsSelectors[0].IsSelected = ExtraOptionsButtonsSelectors[0].IsEnabled ? IsMicroTasksBtnSelected : false;
                ExtraOptionsButtonsSelectors[1].IsEnabled = _subEventType?.IsValueType ?? true;
                ExtraOptionsButtonsSelectors[1].IsSelected = ExtraOptionsButtonsSelectors[1].IsEnabled ? IsValueBtnSelected : false;
                ExtraOptionsButtonsSelectors[2].IsSelected = IsDateBtnSelected;
            }
		}
        public bool CheckIsMicroTaskType()
        {
            return _subEventType != null && _subEventType.IsMicroTaskType != null && _subEventType.IsMicroTaskType == true;
        }
        public bool CheckIsValueType()
        {
            return _subEventType != null && _subEventType.IsValueType != null && _subEventType.IsValueType == true;
        }
        private void OnIsMicroTasksSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsMicroTasksBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsEventValueTypeCommand(SelectableButtonViewModel clickedButton)
		{
			IsValueBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsDateControlsSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsDateBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);
		}

		private void SetPropperValueType()
		{
			DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(_subEventType);
		}

		internal void OnEventTypeChanged(ISubEventTypeModel selectedEventType)	
		{
			_subEventType = selectedEventType;
			InitializeCommon();
			IsEventMicroTasksType = selectedEventType.IsMicroTaskType ? true : false;
			if (IsEventMicroTasksType)
			{
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(selectedEventType.MicroTasksList);
			}


			IsEventValueType = selectedEventType.IsValueType ? true : false;
			if (IsEventValueType)
			{
				DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
				// TODO chcange this so it will look for types in similair families (kg, g, mg, etc...)
				var measurementUnitsForSelectedType = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(unit => unit.TypeOfMeasurementUnit == selectedEventType.QuantityAmount.Unit); // TO CHECK!
				DefaultMeasurementSelectorCCHelper.QuantityAmount = selectedEventType.QuantityAmount;

				DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC = new ObservableCollection<MeasurementUnitItem>(measurementUnitsForSelectedType);
				DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(selectedEventType);
				OnPropertyChanged(nameof(DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC));
			}
			else
			{
				DefaultMeasurementSelectorCCHelper.QuantityAmount = null;
			}

		}
	}
}
