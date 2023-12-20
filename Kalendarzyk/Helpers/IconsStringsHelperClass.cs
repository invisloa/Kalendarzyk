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

		public static ObservableCollection<string> GetActivitiesIcons()
		{
			return new ObservableCollection<string>(ActivitiesIcons);
		}
		public static ObservableCollection<string> GetOthersIcons()
		{
			return new ObservableCollection<string>(OtherIcons);
		}
		public static ObservableCollection<string> GetTopIcons()
		{
			return new ObservableCollection<string>(TopIcons);
		}
		public static ObservableCollection<string> GetItIcons()
		{
			return new ObservableCollection<string>(ItIcons);
		}
		public static ObservableCollection<string> GetTravelIcons()
		{
			return new ObservableCollection<string>(TravelIcons);
		}
		public static readonly List<string> ItIcons = new List<string> // todo change those lists to real ones
		{
			IconFont.Save,
			IconFont.Smartphone,
			IconFont.Print,
			IconFont.Computer,
			IconFont.Desktop_windows,
			IconFont.Headphones,
			IconFont.Headset_mic,
			IconFont.Smart_toy,
			IconFont.Memory,
			IconFont.Keyboard,
			IconFont.Mouse,
			IconFont.Tv,
			IconFont.Router,
			IconFont.Screen_search_desktop,
			IconFont.Wifi,
		};

		public static readonly List<string> TravelIcons = new List<string> // todo change those lists to real ones
		{
			IconFont.Luggage,
			IconFont.Fitness_center,
			IconFont.Lunch_dining,
			IconFont.Cottage,
			IconFont.Local_airport,
			IconFont.Local_cafe,
			IconFont.Hotel,
			IconFont.Family_restroom,
			IconFont.Beach_access,
			IconFont.Liquor,
			IconFont.Airplane_ticket,
			IconFont.Ramen_dining,
			IconFont.Icecream,
			IconFont.Festival,
			IconFont.Attractions,
		};


		public static readonly List<string> ActivitiesIcons = new List<string> // todo change those lists to real ones
		{
			IconFont.Volunteer_activism,
			IconFont.School,
			IconFont.Construction,
			IconFont.Sports_football,
			IconFont.Sports_martial_arts,
			IconFont.Sports_motorsports,
			IconFont.Exercise,
			IconFont.Backpack,
			IconFont.Sports_golf,
			IconFont.Sports_gymnastics,
			IconFont.Sports_tennis,
			IconFont.Sports_esports,
			IconFont.Water,
			IconFont.Air,
			IconFont.Campaign,
		};
		public static readonly List<string> OtherIcons = new List<string>
		{
			IconFont.Dark_mode,
			IconFont.Light_mode,
			IconFont.Watch,
			IconFont.Space_dashboard,
			IconFont.Gif,
			IconFont.Healing,
			IconFont.Storm,
			IconFont.Toys,
			IconFont.Flight,
			IconFont.Electric_scooter,
			IconFont.Refresh,
			IconFont._4k,
			IconFont.Do_not_step,
			IconFont.Fast_forward,
			IconFont.Playlist_play,
		};
		public static readonly List<string> TopIcons = new List<string>
		{
			IconFont.Search,
			IconFont.Home,
			IconFont.Menu,
			IconFont.Close,
			IconFont.Settings,
			IconFont.Done,
			IconFont.Expand_more,
			IconFont.Mic,
			IconFont.Favorite,
			IconFont.Photo_camera,
			IconFont.Delete,
			IconFont.Star,
			IconFont.Cancel,
			IconFont.Bolt,
			IconFont.Timer,     
		};
	}
}
