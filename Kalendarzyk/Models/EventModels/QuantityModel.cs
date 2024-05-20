using Kalendarzyk.ViewModels;
using System.ComponentModel;
using System.Globalization;

namespace Kalendarzyk.Models.EventModels
{
	public class Quantity
	{
		public decimal Value { get; set; }
		public MeasurementUnit Unit { get; set; }

		public Quantity(MeasurementUnit unit, decimal value)
		{
			Value = value;
			Unit = unit;
		}

		public Quantity ConvertTo(MeasurementUnit targetUnit)
		{
			if (Unit == targetUnit)
				return this;

			switch (Unit)
			{
				case MeasurementUnit.Kilogram when targetUnit == MeasurementUnit.Gram:
					return new Quantity(targetUnit, Value * 1000);
				case MeasurementUnit.Kilogram when targetUnit == MeasurementUnit.Milligram:
					return new Quantity(targetUnit, Value * 1000000);
				case MeasurementUnit.Gram when targetUnit == MeasurementUnit.Kilogram:
					return new Quantity(targetUnit, Value / 1000);
				case MeasurementUnit.Gram when targetUnit == MeasurementUnit.Milligram:
					return new Quantity(targetUnit, Value * 1000);
				case MeasurementUnit.Milligram when targetUnit == MeasurementUnit.Gram:
					return new Quantity(targetUnit, Value / 1000);
				case MeasurementUnit.Milligram when targetUnit == MeasurementUnit.Kilogram:
					return new Quantity(targetUnit, Value / 1000000);
				case MeasurementUnit.Liter when targetUnit == MeasurementUnit.Milliliter:
					return new Quantity(targetUnit, Value * 1000);
				case MeasurementUnit.Milliliter when targetUnit == MeasurementUnit.Liter:
					return new Quantity(targetUnit, Value / 1000);
				case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Centimeter:
					return new Quantity(targetUnit, Value * 100);
				case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Millimeter:
					return new Quantity(targetUnit, Value * 1000);
				case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Kilometer:
					return new Quantity(targetUnit, Value / 1000);
				case MeasurementUnit.Centimeter when targetUnit == MeasurementUnit.Meter:
					return new Quantity(targetUnit, Value / 100);
				case MeasurementUnit.Centimeter when targetUnit == MeasurementUnit.Millimeter:
					return new Quantity(targetUnit, Value * 10);
				case MeasurementUnit.Millimeter when targetUnit == MeasurementUnit.Meter:
					return new Quantity(targetUnit, Value / 1000);
				case MeasurementUnit.Millimeter when targetUnit == MeasurementUnit.Centimeter:
					return new Quantity(targetUnit, Value / 10);
				case MeasurementUnit.Kilometer when targetUnit == MeasurementUnit.Meter:
					return new Quantity(targetUnit, Value * 1000);
				case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.SquareKilometer:
					return new Quantity(targetUnit, Value / 1_000_000);
				case MeasurementUnit.SquareKilometer when targetUnit == MeasurementUnit.SquareMeter:
					return new Quantity(targetUnit, Value * 1_000_000);
				case MeasurementUnit.Are when targetUnit == MeasurementUnit.SquareMeter:
					return new Quantity(targetUnit, Value * 100);
				case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.Are:
					return new Quantity(targetUnit, Value / 100);
				case MeasurementUnit.Hectare when targetUnit == MeasurementUnit.SquareMeter:
					return new Quantity(targetUnit, Value * 10_000);
				case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.Hectare:
					return new Quantity(targetUnit, Value / 10_000);
			}
			throw new Exception($"Conversion from {Unit} to {targetUnit} not defined.");
		}

		public Quantity Add(Quantity other)
		{
			if (this.Unit == other.Unit)
			{
				return new Quantity(this.Unit, this.Value + other.Value);
			}
			else
			{
				Quantity otherConverted = other.ConvertTo(this.Unit);
				return new Quantity(this.Unit, this.Value + otherConverted.Value);
			}
		}

		public Quantity Subtract(Quantity other)
		{
			if (this.Unit == other.Unit)
			{
				return new Quantity(this.Unit, this.Value - other.Value);
			}
			else
			{
				Quantity otherConverted = other.ConvertTo(this.Unit);
				return new Quantity(this.Unit, this.Value - otherConverted.Value);
			}
		}

		public Quantity Multiply(decimal factor)
		{
			return new Quantity(this.Unit, this.Value * factor);
		}

		public Quantity Divide(decimal divisor)
		{
			if (divisor == 0) throw new DivideByZeroException();
			return new Quantity(this.Unit, this.Value / divisor);
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

	public class MeasurementUnitItem
	{
		public MeasurementUnitItem(MeasurementUnit unit)
		{
			TypeOfMeasurementUnit = unit;
			DisplayName = unit.GetDescription();
		}

		public MeasurementUnit TypeOfMeasurementUnit { get; set; }
		public string DisplayName { get; set; }
	}

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
	public class QuantityViewModel : BaseViewModel
	{
		private Quantity _quantity;

		public decimal Value
		{
			get => _quantity.Value;
			set
			{
				if (_quantity.Value == value)
				{
					return;
				}
				_quantity.Value = value;
				OnPropertyChanged();
			}
		}

		public MeasurementUnit Unit
		{
			get => _quantity.Unit;
			set
			{
				if (_quantity.Unit == value)
				{
					return;
				}
				_quantity.Unit = value;
				OnPropertyChanged();
			}
		}

		public QuantityViewModel(Quantity quantity)
		{
			_quantity = quantity;
		}

		public Quantity ConvertTo(MeasurementUnit targetUnit) => _quantity.ConvertTo(targetUnit);
		public Quantity Add(Quantity other) => _quantity.Add(other);
		public Quantity Subtract(Quantity other) => _quantity.Subtract(other);
		public Quantity Multiply(decimal factor) => _quantity.Multiply(factor);
		public Quantity Divide(decimal divisor) => _quantity.Divide(divisor);
	}
}
