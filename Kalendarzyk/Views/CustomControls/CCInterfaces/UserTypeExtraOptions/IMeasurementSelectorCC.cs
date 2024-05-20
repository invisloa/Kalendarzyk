using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions
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
		Quantity QuantityAmount { get; set; }
		public string QuantityValueText { get; set; }
		public decimal QuantityValue { get; set; }
		void SelectPropperMeasurementData(ISubEventTypeModel userEventTypeModel);
	}
}
