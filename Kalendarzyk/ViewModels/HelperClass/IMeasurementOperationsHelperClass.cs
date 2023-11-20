using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;

namespace Kalendarzyk.ViewModels.HelperClass
{
	public interface IMeasurementOperationsHelperClass
	{
		decimal AverageOfMeasurements { get; set; }
		decimal MaxOfMeasurements { get; set; }
		decimal MedianOfMeasurements { get; set; }
		decimal MinOfMeasurements { get; set; }
		decimal TotalOfMeasurements { get; set; }
		DateTime DateTo { get; set; }
		DateTime DateFrom { get; set; }
		public bool CheckIfEventsAreSameType();
		void DoBasicCalculations();
		MeasurementCalculationsOutcome MinByWeekCalculation();
		MeasurementCalculationsOutcome MaxByWeekCalculation();
	}
}