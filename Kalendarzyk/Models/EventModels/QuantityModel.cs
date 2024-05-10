using System.ComponentModel;
using System.Globalization;

namespace Kalendarzyk.Models.EventModels
{
	public class QuantityModel
	{
		public decimal Value { get; set; }
		public MeasurementUnit Unit { get; set; }

		public QuantityModel(MeasurementUnit unit, decimal value)
		{
			Value = value;
			Unit = unit;
		}

		public QuantityModel ConvertTo(MeasurementUnit targetUnit)
		{
			if (Unit == targetUnit)
				return this;

			switch (Unit)
			{
				case MeasurementUnit.Kilogram when targetUnit == MeasurementUnit.Gram:
					return new QuantityModel(targetUnit, Value * 1000);
				case MeasurementUnit.Kilogram when targetUnit == MeasurementUnit.Milligram:
					return new QuantityModel(targetUnit, Value * 1000000);
				case MeasurementUnit.Gram when targetUnit == MeasurementUnit.Kilogram:
					return new QuantityModel(targetUnit, Value / 1000);
				case MeasurementUnit.Gram when targetUnit == MeasurementUnit.Milligram:
					return new QuantityModel(targetUnit, Value * 1000);
				case MeasurementUnit.Milligram when targetUnit == MeasurementUnit.Gram:
					return new QuantityModel(targetUnit, Value / 1000);
				case MeasurementUnit.Milligram when targetUnit == MeasurementUnit.Kilogram:
					return new QuantityModel(targetUnit, Value / 1000000);
				case MeasurementUnit.Liter when targetUnit == MeasurementUnit.Milliliter:
					return new QuantityModel(targetUnit, Value * 1000);
				case MeasurementUnit.Milliliter when targetUnit == MeasurementUnit.Liter:
					return new QuantityModel(targetUnit, Value / 1000);
				case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Centimeter:
					return new QuantityModel(targetUnit, Value * 100);
				case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Millimeter:
					return new QuantityModel(targetUnit, Value * 1000);
				case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Kilometer:
					return new QuantityModel(targetUnit, Value / 1000);
				case MeasurementUnit.Centimeter when targetUnit == MeasurementUnit.Meter:
					return new QuantityModel(targetUnit, Value / 100);
				case MeasurementUnit.Centimeter when targetUnit == MeasurementUnit.Millimeter:
					return new QuantityModel(targetUnit, Value * 10);
				case MeasurementUnit.Millimeter when targetUnit == MeasurementUnit.Meter:
					return new QuantityModel(targetUnit, Value / 1000);
				case MeasurementUnit.Millimeter when targetUnit == MeasurementUnit.Centimeter:
					return new QuantityModel(targetUnit, Value / 10);
				case MeasurementUnit.Kilometer when targetUnit == MeasurementUnit.Meter:
					return new QuantityModel(targetUnit, Value * 1000);
				case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.SquareKilometer:
					return new QuantityModel(targetUnit, Value / 1_000_000);
				case MeasurementUnit.SquareKilometer when targetUnit == MeasurementUnit.SquareMeter:
					return new QuantityModel(targetUnit, Value * 1_000_000);
				case MeasurementUnit.Are when targetUnit == MeasurementUnit.SquareMeter:
					return new QuantityModel(targetUnit, Value * 100);
				case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.Are:
					return new QuantityModel(targetUnit, Value / 100);
				case MeasurementUnit.Hectare when targetUnit == MeasurementUnit.SquareMeter:
					return new QuantityModel(targetUnit, Value * 10_000);
				case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.Hectare:
					return new QuantityModel(targetUnit, Value / 10_000);
			}
			// if the exception is thrown, show message to the user he tries to convert from incompatible units
			throw new Exception($"Conversion from {Unit} to {targetUnit} not defined.");
		}

		public QuantityModel Add(QuantityModel other)
		{
			if (this.Unit == other.Unit)
			{
				return new QuantityModel(this.Unit, this.Value + other.Value);
			}
			else
			{
				QuantityModel otherConverted = other.ConvertTo(this.Unit);
				return new QuantityModel(this.Unit, this.Value + otherConverted.Value);
			}
		}

		public QuantityModel Subtract(QuantityModel other)
		{
			if (this.Unit == other.Unit)
			{
				return new QuantityModel(this.Unit, this.Value - other.Value);
			}
			else
			{
				QuantityModel otherConverted = other.ConvertTo(this.Unit);
				return new QuantityModel(this.Unit, this.Value - otherConverted.Value);
			}
		}

		public QuantityModel Multiply(decimal factor)
		{
			return new QuantityModel(this.Unit, this.Value * factor);
		}

		public QuantityModel Divide(decimal divisor)
		{
			if (divisor == 0) throw new DivideByZeroException();
			return new QuantityModel(this.Unit, this.Value / divisor);
		}
	}
}

public enum MeasurementUnit
{
	[Description("Currency")]
	Money,
	[Description("mg")]
	Milligram,
	[Description("g")]
	Gram,
	[Description("kg")]
	Kilogram,

	[Description("ml")]
	Milliliter,
	[Description("L")]
	Liter,

	[Description("cm")]
	Centimeter,
	[Description("mm")]
	Millimeter,
	[Description("m")]
	Meter,
	[Description("km")]
	Kilometer,


	[Description("Week")]
	Week,
	[Description("Day")]
	Day,
	[Description("Hour")]
	Hour,
	[Description("Minute")]
	Minute,
	[Description("Second")]
	Second,




	[Description("Square Meter (m²)")]
	SquareMeter,
	[Description("Square Kilometer (km²)")]
	SquareKilometer,
	[Description("Are (a)")]
	Are,
	[Description("Hectare (ha)")]
	Hectare,


	[Description("Celsius")]
	Celsius,
	[Description("Fahrenheit")]
	Fahrenheit,
	[Description("Kelvin")]
	Kelvin,


}

// helper class
public class MeasurementUnitItem
{
	public MeasurementUnitItem(MeasurementUnit unit)
	{
		TypeOfMeasurementUnit = unit;
		DisplayName = unit.GetDescription(); // using extension method
	}

	public MeasurementUnit TypeOfMeasurementUnit { get; set; }
	public string DisplayName { get; set; }
}


// Extension method for MeasurementUnitItem to get the description attribute
public static class MeasurementUnitExtensions
{
	public static string GetDescription(this MeasurementUnit unit)
	{
		if (unit == MeasurementUnit.Money)
		{
			return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
		}

		var type = unit.GetType();
		var memberInfo = type.GetMember(unit.ToString());
		var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
		return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : unit.ToString();
	}
}