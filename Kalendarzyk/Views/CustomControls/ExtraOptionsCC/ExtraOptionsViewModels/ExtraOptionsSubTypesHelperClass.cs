﻿using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.ViewModels;
using Kalendarzyk.Views.CustomControls.ExtraOptionsCC.ExtraOptionsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalendarzyk.Services;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using CommunityToolkit.Maui.Core.Extensions;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	 public partial class ExtraOptionsSubTypesHelperClass : ExtraOptionsBaseClass
	{
		//ctor create mode
		public ExtraOptionsSubTypesHelperClass()
		{
			InitializeCommon();
		}
		//ctor edit mode
		public ExtraOptionsSubTypesHelperClass(ISubEventTypeModel subEventTypeModel)
		{
			SubEventType = subEventTypeModel;

			InitializeCommon();


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
		private void InitializeCommon() // TODO JO XXX REFACTOR THIS to be more modular
		{
			if (ExtraOptionsButtonsSelectors.Count == 0)
			{
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand), isEnabled: true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsEventValueTypeCommand), isEnabled: true));
			}
			else
			{
				// TODO JO XXX  just for testing this might never be executed if so remove it
				throw new Exception("ExtraOptionsButtonsSelectors.Count != 0");

				ExtraOptionsButtonsSelectors[0].IsEnabled = SubEventType?.IsMicroTaskType ?? true;
				ExtraOptionsButtonsSelectors[0].IsSelected = ExtraOptionsButtonsSelectors[0].IsEnabled ? IsMicroTasksBtnSelected : false;
				ExtraOptionsButtonsSelectors[1].IsEnabled = SubEventType?.IsValueType ?? true;
				ExtraOptionsButtonsSelectors[1].IsSelected = ExtraOptionsButtonsSelectors[1].IsEnabled ? IsValueBtnSelected : false;
			}
		}

	}
}