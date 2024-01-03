using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Service.Autofill;
using Kalendarzyk.Services;

namespace Kalendarzyk
{
	[Activity(Label = "FormSecondApp", Theme = "@style/MainTheme", MainLauncher = true, Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[IntentFilter
	(
	new[] { Android.Content.Intent.ActionView, Android.Content.Intent.ActionSend },
	Categories = new[]
	   {
			 Android.Content.Intent.CategoryDefault
	   },

	DataMimeType = "application/json"
	)]
	public class MainActivity : MauiAppCompatActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (Intent?.Action == Android.Content.Intent.ActionOpenDocument)
			{
				Stream? inputStream = null;
				var filePath = Intent?.ClipData?.GetItemAt(0);
				if (filePath?.Uri != null)
				{
					inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
				}

				if (inputStream != null)
				{
					using (var reader = new StreamReader(inputStream))
					{
						var content = reader.ReadToEnd();

						// TODO HERE
					}

					inputStream.Close();
					inputStream.Dispose();
				}
			}
		}
	}
}
