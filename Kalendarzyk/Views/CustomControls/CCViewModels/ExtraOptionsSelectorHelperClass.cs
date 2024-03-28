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
		private IsCompletedCCViewModel _isCompletedCCAdapter;
		private bool _isCompleted;
		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _extraOptionsButtonsSelectors;
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

			_isCompletedCCAdapter = Factory.CreateNewIsCompletedCCAdapter(_isCompleted);

			if (_eventToEdit.EventType.IsValueType)
			{
				IsEventValueType = true;
				DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
				DefaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
				if (eventToEdit.QuantityAmount != null && eventToEdit.QuantityAmount.Value != 0)
				{
					OnIsEventValueTypeCommand(ExtraOptionsButtonsSelectors[1]); // TODO refactor this
					DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == _eventToEdit.QuantityAmount.Unit).First();
					DefaultMeasurementSelectorCCHelper.QuantityValue = _eventToEdit.QuantityAmount.Value;
				}
				DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(_subEventType);
			}

			if (_eventToEdit.EventType.IsMicroTaskType)
			{
				IsEventMicroTasksType = true;
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTaskModel>());

				if (_eventToEdit.MicroTasksList != null && _eventToEdit.MicroTasksList.Count() > 0)
				{
					OnIsMicroTasksSelectedCommand(ExtraOptionsButtonsSelectors[0]); // TODO refactor this
					MicroTasksCCAdapter.MicroTasksOC = _eventToEdit.MicroTasksList.ToObservableCollection();
				}
			}


		}
		private void InitializeCommon()
		{
			ExtraOptionsButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>		// TODO JO XXX REFACTOR THIS to be more modular
			{
				new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand), isEnabled: _subEventType?.IsMicroTaskType ?? true),
				new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsEventValueTypeCommand),  isEnabled: _subEventType?.IsValueType ?? true ),
				new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnIsDateControlsSelectedCommand))
			};
		}

        private void OnIsMicroTasksSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			//IsEventMicroTasksType = !clickedButton.IsSelected;
			IsMicroTasksBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsEventValueTypeCommand(SelectableButtonViewModel clickedButton)
		{
			//IsEventValueType = !clickedButton.IsSelected;
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
			DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(_subEventType); // TODO XXX
/*		TODO NOW XXX
 *		_subEventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == event;
			var measurementUnitsForSelectedType = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(unit => _subEventType.TypeOfMeasurementUnit == qNoteSubType.DefaultQuantityAmount.Unit); // TO CHECK!
			DefaultMeasurementSelectorCCHelper.QuantityAmount = qNoteSubType.DefaultQuantityAmount;
			DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC = new ObservableCollection<MeasurementUnitItem>(measurementUnitsForSelectedType);
			DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = measurementUnitsForSelectedType.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == qNoteSubType.DefaultQuantityAmount.Unit);
			OnPropertyChanged(nameof(DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC));
*/
		}

		internal void OnEventTypeChanged(ISubEventTypeModel selectedEventType)	// TODO HERE 22.03.24 just added this one
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
				var measurementUnitsForSelectedType = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(unit => unit.TypeOfMeasurementUnit == selectedEventType.DefaultQuantityAmount.Unit); // TO CHECK!
				DefaultMeasurementSelectorCCHelper.QuantityAmount = selectedEventType.DefaultQuantityAmount;

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
