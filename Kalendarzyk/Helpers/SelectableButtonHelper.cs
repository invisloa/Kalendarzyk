using Kalendarzyk.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kalendarzyk.Helpers
{
	public static class SelectableButtonHelper
	{
		private static readonly int numberOfShades = 10; // Fixed number of shades to generate

		// Base colors from which shades will be generated
		private static readonly List<Color> _baseColors = new List<Color>()
		{
			Color.FromRgb(255, 0, 0), Color.FromRgb(205, 92, 92),
			Color.FromRgb(0, 128, 0), Color.FromRgb(60, 179, 113),
			Color.FromRgb(0, 0, 255), Color.FromRgb(30, 144, 255),
			Color.FromRgb(255, 165, 0), Color.FromRgb(255, 215, 0)
		};

		public static ObservableCollection<SelectableButtonViewModel> GenerateColorPaletteButtons()
		{
			var allColorShades = ColorShadesGenerator.GenerateColorShades(_baseColors, numberOfShades);
			var buttonsColorsOC = new ObservableCollection<SelectableButtonViewModel>();

			foreach (var color in allColorShades)
			{
				buttonsColorsOC.Add(new SelectableButtonViewModel { ButtonColor = color });
			}

			return buttonsColorsOC;
		}
		public static ObservableCollection<SelectableButtonViewModel> GenerateColorPaletteButtons(ICommand selectButtonCommand)
		{
			var allColorShades = ColorShadesGenerator.GenerateColorShades(_baseColors, numberOfShades);
			var buttonsColorsOC = new ObservableCollection<SelectableButtonViewModel>();

			foreach (var color in allColorShades)
			{
				buttonsColorsOC.Add(new SelectableButtonViewModel { ButtonColor = color, ButtonCommand = selectButtonCommand });

			}

			return buttonsColorsOC;
		}
	}
}
