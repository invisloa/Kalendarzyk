namespace Kalendarzyk.Helpers
{
	public static class ColorShadesGenerator
	{
		public static List<Color> GenerateColorShades(List<Color> baseColors, int numberOfShades)
		{
			var allColorShades = new List<Color>();

			foreach (var baseColor in baseColors)
			{
				allColorShades.AddRange(GenerateShadesForBaseColor(baseColor, numberOfShades));
			}

			return allColorShades;
		}

		private static IEnumerable<Color> GenerateShadesForBaseColor(Color baseColor, int numberOfShades)
		{
			var shades = new List<Color>();
			double step = 1.0 / (numberOfShades + 1);

			for (int i = 1; i <= numberOfShades; i++)
			{
				double factor = 1.0 - step * i;

				double red = Math.Max(0, Math.Min(baseColor.Red * factor, 1.0));
				double green = Math.Max(0, Math.Min(baseColor.Green * factor, 1.0));
				double blue = Math.Max(0, Math.Min(baseColor.Blue * factor, 1.0));

				shades.Add(Color.FromRgba(red, green, blue, 1));
			}

			return shades;
		}
	}
}