using Kalendarzyk.Models.EventModels;
using Kalendarzyk.ViewModels.HelperClass.ExtensionsMethods;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Kalendarzyk.ViewModels.HelperClass
{
	public class MeasurementOperationsHelperClass : IMeasurementOperationsHelperClass
	{
		public decimal TotalOfMeasurements { get; set; }
		public decimal AverageOfMeasurements { get; set; }
		public decimal MaxOfMeasurements { get; set; }
		public decimal MinOfMeasurements { get; set; }
		public decimal MedianOfMeasurements { get; set; }
		// Average by Day
		public decimal AverageByDay { get; set; }
		public decimal AverageByWeek { get; set; }
		public decimal AverageByMonth { get; set; }
		public decimal AverageByYear { get; set; }
		public decimal SumByDay { get; set; }
		public decimal SumByWeek { get; set; }
		public decimal SumByMonth { get; set; }
		public decimal SumByYear { get; set; }
		public decimal MaxByDay { get; set; }
		public List<DateTime> MaxByDayDatesList { get; set; }
		public decimal MaxByWeek { get; set; }
		public List<(int, int)> MaxByWeekInfoList { get; set; }
		public decimal MaxByMonth { get; set; }
		public decimal MaxByYear { get; set; }
		public decimal MinByDay { get; set; }
		public List<DateTime> MinByDayDatesList { get; set; }
		public decimal MinByWeek { get; set; }
		public List<(int, int)> MinByWeekInfoList { get; set; }
		public decimal MinByMonth { get; set; }
		public decimal MinByYear { get; set; }
		public DateTime DateTo { get; set; }
		public DateTime DateFrom { get; set; }

		private List<MeasurementUnit> MoneyTypeMeasurements { get; set; }
		private List<MeasurementUnit> WeightTypeMeasurements { get; set; }
		private List<MeasurementUnit> VolumeTypeMeasurements { get; set; }
		private List<MeasurementUnit> DistanceTypeMeasurements { get; set; }
		private List<MeasurementUnit> TimeTypeMeasurements { get; set; }
		private List<MeasurementUnit> TemperatureTypeMeasurements { get; set; }
		private ObservableCollection<IGeneralEventModel> _eventsToShowOC { get; set; }
		private List<IGeneralEventModel> _eventsOrderedByDateList
		{
			get
			{
				return _eventsToShowOC.OrderBy(e => e.StartDateTime).ToList();
			}
		}

		Dictionary<MeasurementUnit, List<MeasurementUnit>> measurementTypeMap;

		// CTOR
		public MeasurementOperationsHelperClass(ObservableCollection<IGeneralEventModel> eventsToCalculateList)
		{
			MoneyTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Money };
			WeightTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Gram, MeasurementUnit.Kilogram, MeasurementUnit.Milligram };
			VolumeTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Liter, MeasurementUnit.Milliliter };
			DistanceTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Centimeter, MeasurementUnit.Kilometer, MeasurementUnit.Meter, MeasurementUnit.Millimeter };
			TimeTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Week, MeasurementUnit.Day, MeasurementUnit.Hour, MeasurementUnit.Minute, MeasurementUnit.Second };
			TemperatureTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Celsius, MeasurementUnit.Fahrenheit, MeasurementUnit.Kelvin };
			measurementTypeMap = InitializeMappingDictionary();
			_eventsToShowOC = eventsToCalculateList;
		}


		// Basic Calculations
		public void DoBasicCalculations()
		{

			// Perform operations if all events are the same type
			TotalOfMeasurements = _eventsOrderedByDateList.Sum(x => x.QuantityAmount.Value);
			AverageOfMeasurements = _eventsOrderedByDateList.Average(x => x.QuantityAmount.Value);
			MaxOfMeasurements = _eventsOrderedByDateList.Max(x => x.QuantityAmount.Value);
			MinOfMeasurements = _eventsOrderedByDateList.Min(x => x.QuantityAmount.Value);


			// Calculate by Period (using the DateTime parameters)
			int days = (DateTo - DateFrom).Days;
			int weeks = days / 7; // Simplified, consider using a more accurate method
			int months = days / 30; // Simplified
			int years = days / 365; // Simplified

			//if not full period time return amount by itself
			// TO DO TODO Change the below to better calculate the amount by period
			AverageByDay = (days != 0) ? TotalOfMeasurements / days : TotalOfMeasurements;
			AverageByWeek = (weeks != 0) ? TotalOfMeasurements / weeks : TotalOfMeasurements;
			AverageByMonth = (months != 0) ? TotalOfMeasurements / months : TotalOfMeasurements;
			AverageByYear = (years != 0) ? TotalOfMeasurements / years : TotalOfMeasurements;

		}
		public bool CheckIfEventsAreSameType()
		{
			if (_eventsOrderedByDateList.Count == 0)
			{
				return false;
			}
			else
			{
				var firstEvent = _eventsOrderedByDateList[0];
				var firstEventUnit = firstEvent.QuantityAmount.Unit;
				if (!measurementTypeMap.ContainsKey(firstEventUnit))
				{
					throw new Exception("Unsupported measurement unit.");
				}
				var measurementTypeList = measurementTypeMap[firstEventUnit];

				foreach (var item in _eventsOrderedByDateList)
				{
					if (!measurementTypeList.Contains(item.QuantityAmount.Unit))
					{
						// if any event has a different measurement type => false => dont do calculations
						return false;
					}
				}
				return true;
			}
		}

		// TODO mapping is not considered in calculations for now!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		private Dictionary<MeasurementUnit, List<MeasurementUnit>> InitializeMappingDictionary()
		{
			return new Dictionary<MeasurementUnit, List<MeasurementUnit>>
			{
					{ MeasurementUnit.Money, MoneyTypeMeasurements },
					{ MeasurementUnit.Gram, WeightTypeMeasurements },
					{ MeasurementUnit.Kilogram, WeightTypeMeasurements },
					{ MeasurementUnit.Milligram, WeightTypeMeasurements },
					{ MeasurementUnit.Liter, VolumeTypeMeasurements },
					{ MeasurementUnit.Milliliter, VolumeTypeMeasurements },
					{ MeasurementUnit.Centimeter, DistanceTypeMeasurements },
					{ MeasurementUnit.Kilometer, DistanceTypeMeasurements },
					{ MeasurementUnit.Meter, DistanceTypeMeasurements },
					{ MeasurementUnit.Millimeter, DistanceTypeMeasurements },
					{ MeasurementUnit.Week, TimeTypeMeasurements },
					{ MeasurementUnit.Day, TimeTypeMeasurements },
					{ MeasurementUnit.Hour, TimeTypeMeasurements },
					{ MeasurementUnit.Minute, TimeTypeMeasurements },
					{ MeasurementUnit.Second, TimeTypeMeasurements },
					{ MeasurementUnit.Celsius, TemperatureTypeMeasurements },
					{ MeasurementUnit.Fahrenheit, TemperatureTypeMeasurements },
					{ MeasurementUnit.Kelvin, TemperatureTypeMeasurements }
				};
		}

		// ADVANCED MEASUREMENT METHODS
		// It is ok to return only datelist, because if i go to any date in the list i will still get to a propper events list
		// week => gotoweekpage, month => gotomonthpage
		#region AdvancedMeasurementMethods

		// Define your delegate and comparison types
		public delegate void UpdateBySpecifiedTimeFrameDelegate(ref decimal currentPeriodTotal, ref decimal extremeValueForPeriod, DateTime currentPeriodDate, List<DateTime> daysWithExtremeValuesList, ComparisonDelegate comparison);
		public delegate bool ComparisonDelegate(decimal value1, decimal value2);

		// Generalized function to calculate by day or by week
		private MeasurementCalculationsOutcome CalculateByPeriod(
			UpdateBySpecifiedTimeFrameDelegate updateDelegate,
			ComparisonDelegate comparison,
			decimal initialByDayValue,
			Func<DateTime, int> getSpecifiedPeriodNumber)
		{
			if (_eventsOrderedByDateList == null || !_eventsOrderedByDateList.Any())
			{
				throw new Exception("The list of events is empty.");
			}

			decimal currentPeriodTotal = 0;
			decimal extremeValueForPeriod = initialByDayValue;
			var currentPeriodDate = _eventsOrderedByDateList[0].StartDateTime.Date;
			var datesWithExtremeValuesList = new List<DateTime>() { currentPeriodDate };
			var lastPeriodNumber = getSpecifiedPeriodNumber(_eventsOrderedByDateList[0].StartDateTime.Date);
			MeasurementUnit measurementUnit = _eventsOrderedByDateList[0].QuantityAmount.Unit;

			foreach (var item in _eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // this should not happen
				}

				var itemSpecifiedPeriodNumber = getSpecifiedPeriodNumber(item.StartDateTime.Date);
				if (itemSpecifiedPeriodNumber != lastPeriodNumber)
				{
					updateDelegate(ref currentPeriodTotal, ref extremeValueForPeriod, currentPeriodDate, datesWithExtremeValuesList, comparison);
					currentPeriodTotal = 0;
					lastPeriodNumber = itemSpecifiedPeriodNumber;
					currentPeriodDate = item.StartDateTime.Date;
				}

				currentPeriodTotal += item.QuantityAmount.Value;
			}

			updateDelegate(ref currentPeriodTotal, ref extremeValueForPeriod, currentPeriodDate, datesWithExtremeValuesList, comparison);

			return new MeasurementCalculationsOutcome(extremeValueForPeriod, datesWithExtremeValuesList, measurementUnit);
		}

		// Method to get week number
		private int GetWeekNumber(DateTime date)
		{
			var calendar = CultureInfo.CurrentCulture.Calendar;
			return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}
		// Method to get month number
		private int GetMonthNumber(DateTime date)
		{
			return date.Month; //lambda: date => date.Month;
		}
		// Generalized update method
		private void UpdateByPeriod(ref decimal currentPeriodSum, ref decimal currentByPeriod, DateTime currentPeriodDate, List<DateTime> datesWithExtremeValuesList, ComparisonDelegate comparison)
		{
			if (comparison(currentPeriodSum, currentByPeriod))
			{
				currentByPeriod = currentPeriodSum;
				datesWithExtremeValuesList.Clear();
				datesWithExtremeValuesList.Add(currentPeriodDate);
			}
			else if (currentPeriodSum == currentByPeriod)
			{
				datesWithExtremeValuesList.Add(currentPeriodDate);
			}
		}

		// Max and Min comparison methods remain the same

		// Methods to call
		private bool MaxComparison(decimal currentDaySum, decimal currentMaxByDay)
		{
			return currentDaySum > currentMaxByDay;
		}

		private bool MinComparison(decimal currentDaySum, decimal currentMinByDay)
		{
			return currentDaySum < currentMinByDay;
		}

		public MeasurementCalculationsOutcome MaxByDayCalculation()
		{
			return CalculateByPeriod(UpdateByPeriod, MaxComparison, 0, date => date.Day);
		}

		public MeasurementCalculationsOutcome MinByDayCalculation()
		{
			return CalculateByPeriod(UpdateByPeriod, MinComparison, decimal.MaxValue, date => date.Day);
		}

		public MeasurementCalculationsOutcome MaxByWeekCalculation()
		{
			return CalculateByPeriod(UpdateByPeriod, MaxComparison, 0, GetWeekNumber);
		}

		public MeasurementCalculationsOutcome MinByPeriodCalculationWithEventsOnly(
			Func<DateTime, int> periodNumberGetter,
			Func<DateTime, DateTime> nextPeriodStartGetter,
			DateTime initialDate)
		{
			return CalculateByPeriod(UpdateByPeriod, MinComparison, decimal.MaxValue, periodNumberGetter);
		}
		private MeasurementCalculationsOutcome MinByPeriodCalculationWithEmptyDates(
			Func<DateTime, int> periodNumberGetter,
			Func<DateTime, DateTime> nextPeriodStartGetter,
			DateTime initialDate)
		{
			var result = CalculateByPeriod(UpdateByPeriod, MinComparison, decimal.MaxValue, periodNumberGetter);

			if (result.MeasurementValueOutcome >= 0)
			{
				DateTime currentPeriodStart = initialDate;
				while (currentPeriodStart < DateTo)
				{
					DateTime nextPeriodStart = nextPeriodStartGetter(currentPeriodStart);

					bool hasEventsThisPeriod = _eventsOrderedByDateList
						.Any(e => e.StartDateTime.Date >= currentPeriodStart && e.StartDateTime.Date < nextPeriodStart);

					if (!hasEventsThisPeriod)
					{
						if (result.MeasurementValueOutcome > 0)
						{
							result.MeasurementDatesListOutcome.Clear();
						}
						result.MeasurementValueOutcome = 0;
						result.MeasurementDatesListOutcome.Add(currentPeriodStart);
					}

					currentPeriodStart = nextPeriodStart;
				}
			}
			return result;
		}
		public MeasurementCalculationsOutcome MinByDayCalculationWithEmptyDates()
		{
			return MinByPeriodCalculationWithEmptyDates(date => date.Day, date => date.AddDays(1), DateFrom);
		}
		public MeasurementCalculationsOutcome MinByWeekCalculationWithEmptyDates()
		{
			return MinByPeriodCalculationWithEmptyDates(GetWeekNumber, date => date.NextMonday(), DateFrom);
		}
		public MeasurementCalculationsOutcome MinByMonthCalculationWithEmptyDates()
		{
			return MinByPeriodCalculationWithEmptyDates(GetMonthNumber, date => date.AddMonths(1), new DateTime(DateFrom.Year, DateFrom.Month, 1));
		}
		public MeasurementCalculationsOutcome MinByWeekCalculation()
		{
			return MinByPeriodCalculationWithEventsOnly(GetWeekNumber, date => date.NextMonday(), DateFrom);
		}

		public MeasurementCalculationsOutcome MinByMonthCalculation()
		{
			return MinByPeriodCalculationWithEventsOnly(GetMonthNumber, date => date.AddMonths(1), new DateTime(DateFrom.Year, DateFrom.Month, 1));
		}

		public MeasurementCalculationsOutcome MaxByMonthCalculation()
		{
			return CalculateByPeriod(UpdateByPeriod, MaxComparison, 0, GetMonthNumber);
		}
		#endregion
	}

	#region HelperClass TO MOVE TO SEPARATE FILE

	public class MeasurementCalculationsOutcome
	{
		public MeasurementCalculationsOutcome(decimal measurementValueOutcome, List<DateTime> measurementsEventsOutcome, MeasurementUnit measurementUnitValue)
		{
			MeasurementValueOutcome = measurementValueOutcome;
			MeasurementDatesListOutcome = measurementsEventsOutcome;
			MeasurementUnitValue = new MeasurementUnitItem(measurementUnitValue);
		}
		public MeasurementUnitItem MeasurementUnitValue { get; set; }
		public decimal MeasurementValueOutcome { get; set; }
		public List<DateTime> MeasurementDatesListOutcome { get; set; }
	}
	#endregion
}
