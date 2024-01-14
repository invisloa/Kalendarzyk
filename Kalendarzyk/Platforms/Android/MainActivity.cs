using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Service.Autofill;
using Kalendarzyk.Services;
using Kalendarzyk.Services.EventsSharing;

namespace Kalendarzyk
{

	[Activity(Label = "Kalendarzyk", Theme = "@style/MainTheme", MainLauncher = true, Exported = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

	[IntentFilter(new[] { Android.Content.Intent.ActionView },
		Categories = new[] { Android.Content.Intent.CategoryDefault },
		DataMimeType = "application/json")]

	public class MainActivity : MauiAppCompatActivity
	{
		bool _isIntentHandled;
		IShareEventsService _shareEventsService = Factory.CreateNewShareEventsService();
		// when the app is not running and a new intent is received
		protected override  void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);
		}

		// when the app is running and a new intent is received
		protected override void OnResume()
		{
			base.OnResume();

		}
		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			try
			{
				base.OnPostCreate(savedInstanceState);
			}
			catch (Exception ex)
			{
				Android.Util.Log.Error("MyApp", "Error processing the file: " + ex.ToString());
			}
		}
		protected override void OnNewIntent(Intent intent)
		{
			base.OnNewIntent(intent);
			HandleIntent(intent);
		}

		private void HandleIntent(Intent intent)
		{

			var intentType = intent.Type;
			if (intentType != null)
			{

				switch (intentType)
				{
					case "application/json":
						// Get the action of the intent that started this activity
						var intentAction = intent.Action;

						if (intentAction == Intent.ActionView)  // Check if the action is ACTION_VIEW
						{
							var fileUri = intent.Data;  // Get the URI of the file from the intent's data

							if (fileUri != null)
							{
								try
								{
									// Open an input stream to read the file
									using (Stream inputStream = ContentResolver.OpenInputStream(fileUri))
									using (StreamReader reader = new StreamReader(inputStream))
									{
										var content = reader.ReadToEnd();
										_shareEventsService.ImportEventsAsync(content);
									}
								}
								catch (Exception ex)
								{
									// Handle exceptions here
									Android.Util.Log.Error("MyApp", "Error processing the file: " + ex.ToString());
								}
							}
						}
						break;
					case "text/calendar":
						string json2 = Intent.GetStringExtra(Intent.ExtraText);
						Console.WriteLine(json2); break;

					default:
						// Handle unknown or unsupported file types
						break;
				}
			}

		}

	}
}

