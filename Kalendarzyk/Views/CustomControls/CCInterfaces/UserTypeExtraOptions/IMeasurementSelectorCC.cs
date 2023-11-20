using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces.UserTypeExtraOptions
{

	/// <summary>
	/// Its good to use MeasurementOperationsHelperClass
	/// </summary>
	public interface IMeasurementSelectorCC
	{
		// Properties
		ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC { get; set; }
		RelayCommand<MeasurementUnitItem> MeasurementUnitSelectedCommand { get; set; }
		MeasurementUnitItem SelectedMeasurementUnit { get; set; }
		QuantityModel QuantityAmount { get; set; }
		public string QuantityValueText { get; set; }
		public decimal QuantityValue { get; set; }
		void SelectPropperMeasurementData(ISubEventTypeModel userEventTypeModel);
	}
}
