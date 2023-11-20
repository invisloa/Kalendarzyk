using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Helpers
{
	internal class IconsHelperClass
	{

		public static ObservableCollection<string> GetTopIcons()
		{
			return new ObservableCollection<string>(TopIcons);
		}
		public static ObservableCollection<string> GetTopIcons2()
		{
			return new ObservableCollection<string>(TopIcons2);
		}
		public static ObservableCollection<string> GetTopIcons3()
		{
			return new ObservableCollection<string>(TopIcons3);
		}


		public static readonly List<string> TopIcons = new List<string> // todo change those lists to real ones
		{
			IconFont.Safety_check,
			IconFont.Sailing,
			IconFont.High_quality,
			IconFont.Accessibility,
			IconFont.Accessibility_new,
			IconFont.Accessible
		};
		public static readonly List<string> TopIcons2 = new List<string>
		{
			IconFont.Sanitizer,
			IconFont.Satellite,
			IconFont.Satellite_alt,
			IconFont.Save,
			IconFont.Save_alt,
			IconFont.Saved_search
		};
		public static readonly List<string> TopIcons3 = new List<string>
		{
			IconFont.Dangerous,
			IconFont.Dark_mode,
			IconFont.Dashboard,
			IconFont.Dashboard_customize,
			IconFont.Dataset_linked,
			IconFont.Dataset
		};
	}
}
