using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Service.Autofill;
using Kalendarzyk.Services;

namespace Kalendarzyk
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HandleIntent(Intent);
		}

		protected override void OnNewIntent(Intent intent)
		{
			base.OnNewIntent(intent);
			HandleIntent(intent);
		}
		void HandleIntent(Intent intent)
		{
			if (intent?.Action == Intent.ActionView)
			{
				var contentUri = intent.Data;
				var fileContent = contentUri.ToString();
				var shareEventsService = Factory.CreateNewShareEventsService();
				shareEventsService.ImportEventAsync(fileContent);
			}
		}
	}

}
