using Kalendarzyk.ViewModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Helpers
{
	public class ButtonsColorsInitializerHelperClass
	{
		public ObservableCollection<SelectableButtonViewModel> ButtonsColorsOC { get; set; }
		private int NumberOfColumns = 10;

		public ButtonsColorsInitializerHelperClass()
		{
			InitializeColorButtons();
		}

		private void InitializeColorButtons()
		{
			ButtonsColorsOC = new ObservableCollection<SelectableButtonViewModel>();

			// Define base color groups
			var colorGroups = new List<Color[]>
			{
				new Color[] { Color.FromRgb(255, 0, 0), Color.FromRgb(205, 92, 92) }, // Reds
				new Color[] { Color.FromRgb(0, 128, 0), Color.FromRgb(60, 179, 113) }, // Greens
				new Color[] { Color.FromRgb(0, 0, 255), Color.FromRgb(30, 144, 255) }, // Blues
				new Color[] { Color.FromRgb(255, 165, 0), Color.FromRgb(255, 215, 0) }, // Oranges & Yellows
				// Add more color groups as needed, with each group containing similar colors
			};

			// Randomize the order of the color groups to mix the palette
			var rnd = new Random();
			var randomizedGroups = colorGroups.OrderBy(x => rnd.Next()).ToList();

			// For each group, generate and add shades
			foreach (var group in randomizedGroups)
			{
				foreach (var baseColor in group)
				{
					AddColors(GenerateShades(baseColor));
				}
			}
		}

		private void AddColors(params Color[] colors)
		{
			foreach (var color in colors)
			{
				ButtonsColorsOC.Add(new SelectableButtonViewModel { ButtonColor = color });
			}
		}

		private Color[] GenerateShades(Color baseColor)
		{
			Color[] shades = new Color[NumberOfColumns];
			double lightnessRange = 0.5; // Range for lightening (e.g., from 1.0 for base color to 1.5 for lightest color)
			double darknessRange = 0.5; // Range for darkening (e.g., from 1.0 for base color to 0.5 for darkest color)

			// Calculate the step for each shade based on the number of columns
			double step = (lightnessRange + darknessRange) / (NumberOfColumns - 1);

			for (int i = 0; i < NumberOfColumns; i++)
			{
				// Calculate factor for current shade
				double factor = 1 + lightnessRange - step * i;

				// Clamp factor to not exceed the range of [0,1] after adjustment
				double red = Math.Max(0, Math.Min(baseColor.Red * factor, 1.0));
				double green = Math.Max(0, Math.Min(baseColor.Green * factor, 1.0));
				double blue = Math.Max(0, Math.Min(baseColor.Blue * factor, 1.0));

				shades[i] = Color.FromRgba(
					red,
					green,
					blue,
					1 // Alpha for opacity
				);
			}

			return shades;
		}


	}
}
