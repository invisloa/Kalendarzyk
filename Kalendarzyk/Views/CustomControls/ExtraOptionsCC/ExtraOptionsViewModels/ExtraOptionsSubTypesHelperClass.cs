using CommunityToolkit.Mvvm.ComponentModel;
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
		[ObservableProperty]
		private bool _isColorBtnSelected;

		[ObservableProperty]
		private Command _onIsColorBtnSelectedCommand;


		//ctor create mode
		public ExtraOptionsSubTypesHelperClass()
		{
			//SubEventType = Factory.CreateNewSubEventTypeModel();
			InitializeCommon();
		}
		//ctor edit mode
		public ExtraOptionsSubTypesHelperClass(ISubEventTypeModel subEventTypeModel)
		{
			SubEventType = subEventTypeModel;

			InitializeCommon();
			ExtraOptionsButtonsSelectors[1].IsEnabled = false;

			if (SubEventType.IsValueType)
			{
				IsValueType = true;
				ExtraOptionsButtonsSelectors[1].ButtonCommand = null;
				DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
				DefaultMeasurementSelectorCCHelper.QuantityAmount = new Quantity(DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, DefaultMeasurementSelectorCCHelper.QuantityValue);
				if (SubEventType.QuantityAmount != null )
				{
					OnIsEventValueType(ExtraOptionsButtonsSelectors[1]); // TODO refactor this
					var unitToSelect = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == SubEventType.QuantityAmount.Unit).First();
					DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = unitToSelect;
					DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC = new ObservableCollection<MeasurementUnitItem> { unitToSelect };		// TODO JO REFACTOR THIS, ONLY TEMPORARY - NO TIME :(
					DefaultMeasurementSelectorCCHelper.QuantityValue = SubEventType.QuantityAmount.Value;
				}
				DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(SubEventType);
			}

			if (SubEventType.IsMicroTaskType)
			{
				IsMicroTasksType = true;
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTask>());

				if (SubEventType.MicroTasksList != null && SubEventType.MicroTasksList.Count() > 0)
				{
					OnIsMicroTasksSelected(ExtraOptionsButtonsSelectors[0]); // TODO refactor this
					MicroTasksCCAdapter.MicroTasksOC = SubEventType.MicroTasksList.ToObservableCollection();
				}
			}
			SetPropperValueType();

		}
		private void InitializeCommon() // TODO JO XXX REFACTOR THIS to be more modular
		{
			if (ExtraOptionsButtonsSelectors.Count == 0)
			{
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Micro Tasks(D)", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelected), isEnabled: true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("Value(D)", false, new RelayCommand<SelectableButtonViewModel>(OnIsEventValueType), isEnabled: true));
				ExtraOptionsButtonsSelectors.Add(new SelectableButtonViewModel("BG color", false, new RelayCommand<SelectableButtonViewModel>(OnColorBtnSelected), isEnabled: true));
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
		private void OnColorBtnSelected(SelectableButtonViewModel clickedButton)
		{
			IsColorBtnSelected = UpdateButtonState(clickedButton);
		}
		protected override void OnIsMicroTasksSelected(SelectableButtonViewModel clickedButton)
		{
			if(MicroTasksCCAdapter == null)
			{
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTask>());
			}
			IsMicroTasksBtnSelected = IsMicroTasksType = UpdateButtonState(clickedButton);
		}

		protected override void OnIsEventValueType(SelectableButtonViewModel clickedButton)
		{
			IsValueBtnSelected = IsValueType = UpdateButtonState(clickedButton);
		}
	}
}
