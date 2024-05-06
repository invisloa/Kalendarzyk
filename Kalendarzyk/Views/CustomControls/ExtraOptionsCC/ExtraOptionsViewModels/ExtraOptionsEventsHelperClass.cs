using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.ViewModels;
using Kalendarzyk.Views.CustomControls.ExtraOptionsCC.ExtraOptionsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{

	// TODO HERE 27.03.24 WHEN SWITCHING SUB TYPE IN EVENTS ADDING THE CONTROLS cONTROLS ARE NOT REFRESHED Accordingly
	public partial class ExtraOptionsEventsHelperClass :  ExtraOptionsBaseClass
	{
		[ObservableProperty]
		private bool _isDateBtnSelected;
		//ctor create mode
		public ExtraOptionsEventsHelperClass()
		{
			InitializeExtraOptionsButtons();
		}
		//ctor edit mode
		public ExtraOptionsEventsHelperClass(ISubEventTypeModel subEventTypeModel)
		{
			SubEventType = subEventTypeModel;

            InitializeExtraOptionsButtons();

			if (SubEventType.IsValueType)
			{
				IsValueType = true;
				DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
				DefaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, DefaultMeasurementSelectorCCHelper.QuantityValue);
				if (SubEventType.QuantityAmount != null && SubEventType.QuantityAmount.Value != 0)
				{
					OnIsEventValueTypeCommand(ExtraOptionsButtonsSelectors[1]); // TODO refactor this
					DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == SubEventType.QuantityAmount.Unit).First();
					DefaultMeasurementSelectorCCHelper.QuantityValue = SubEventType.QuantityAmount.Value;
				}
				DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(SubEventType);
			}

			if (SubEventType.IsMicroTaskType)
			{
				IsMicroTasksType = true;
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTaskModel>());

				if (SubEventType.MicroTasksList != null && SubEventType.MicroTasksList.Count() > 0)
				{
					OnIsMicroTasksSelectedCommand(ExtraOptionsButtonsSelectors[0]); // TODO refactor this
					MicroTasksCCAdapter.MicroTasksOC = SubEventType.MicroTasksList.ToObservableCollection();
				}
			}
			SetPropperValueType();

		}
		private void InitializeExtraOptionsButtons() // TODO JO XXX REFACTOR THIS to be more modular
		{
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand), isEnabled: SubEventType?.IsMicroTaskType == true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsEventValueTypeCommand), isEnabled: SubEventType?.IsValueType == true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnIsDateControlsSelectedCommand), isEnabled: SubEventType != null));
		}
		private void ReloadExtraOptionsButtons() // TODO JO XXX REFACTOR THIS to be more modular
		{

			ExtraOptionsButtonsSelectors[0].IsEnabled = SubEventType?.IsMicroTaskType ?? true;
			ExtraOptionsButtonsSelectors[0].IsSelected = IsMicroTasksBtnSelected;
			ExtraOptionsButtonsSelectors[1].IsEnabled = SubEventType?.IsValueType ?? true;
			ExtraOptionsButtonsSelectors[1].IsSelected = IsValueBtnSelected;
			ExtraOptionsButtonsSelectors[2].IsEnabled = true;
			ExtraOptionsButtonsSelectors[2].IsSelected = IsDateBtnSelected;
		}


		private void OnIsDateControlsSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsDateBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);
		}



		internal void OnEventTypeChanged(ISubEventTypeModel selectedEventType)	
		{
			if(selectedEventType == SubEventType)
			{ 			
				return;
			}
			SubEventType = selectedEventType;
			ReloadExtraOptionsButtons();
			IsMicroTasksType = selectedEventType.IsMicroTaskType ? true : false;
			if (IsMicroTasksType)
			{
				if (MicroTasksCCAdapter == null)
				{
					MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(selectedEventType.MicroTasksList);
				}
				else
				{
					MicroTasksCCAdapter.MicroTasksOC = selectedEventType.MicroTasksList.ToObservableCollection();
				}
			}


			IsValueType = selectedEventType.IsValueType ? true : false;
			if (IsValueType)
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
