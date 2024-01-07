﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Service.Autofill;
using Kalendarzyk.Services;
using Kalendarzyk.Services.EventsSharing;

namespace Kalendarzyk
{

	[Activity(Label = "FormSecondApp", Theme = "@style/MainTheme", MainLauncher = true, Exported = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

	[IntentFilter(new[] { Android.Content.Intent.ActionView },
Categories = new[] { Android.Content.Intent.CategoryDefault },
DataMimeType = "application/*", DataPathPattern = "*.Dinero")]


	public class MainActivity : MauiAppCompatActivity
	{
		IShareEventsService _shareEventsService = Factory.CreateNewShareEventsService();
		// when the app is not running and a new intent is received
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Get the action of the intent that started this activity
			var intentAction = Intent.Action;

			if (intentAction == Intent.ActionView)  // Check if the action is ACTION_VIEW
			{
				var fileUri = Intent.Data;  // Get the URI of the file from the intent's data

				if (fileUri != null)
				{
					try
					{
						// Open an input stream to read the file
						using (Stream inputStream = ContentResolver.OpenInputStream(fileUri))
						using (StreamReader reader = new StreamReader(inputStream))
						{
							var content = reader.ReadToEnd();
							_shareEventsService.ImportEventAsync(content);
						}
					}
					catch (Exception ex)
					{
						// Handle exceptions here
						Android.Util.Log.Error("MyApp", "Error processing the file: " + ex.ToString());
					}
				}
			}
		}
		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			try
			{ 				base.OnPostCreate(savedInstanceState);
							HandleIntent(Intent);
						}
			catch (Exception ex)
			{
				Android.Util.Log.Error("MyApp", "Error processing the file: " + ex.ToString());
			}
			HandleIntent(Intent);
		}

		//protected override void OnCreate(Bundle savedInstanceState)
		//{
		//	base.OnCreate(savedInstanceState);
		//	var intentxx = Intent.Action;
		//	if (intentxx != null)
		//	{
		//		if (intentxx == Android.Content.Intent.ActionOpenDocument)
		//		{
		//			Stream? inputStream = null;
		//			var filePath = Intent?.ClipData?.GetItemAt(0);
		//			if (filePath?.Uri != null)
		//			{
		//				inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
		//			}

		//			if (inputStream != null)
		//			{
		//				using (var reader = new StreamReader(inputStream))
		//				{
		//					var content = reader.ReadToEnd();

		//					// TODO HERE
		//				}

		//				inputStream.Close();
		//				inputStream.Dispose();
		//			}
		//		}
		//	}
		//}
		// when the app is already running and a new intent is received
		//protected override void OnNewIntent(Intent intent)
		//{
		//	base.OnNewIntent(intent);
		//	// Update the intent.
		//	Intent = intent;
		//	// Handle the new intent.
		//	HandleIntent(intent);
		//}

		private void HandleIntent(Intent intent)
		{
			var intentType = intent.Type;
			if (intentType != null)
			{

				switch (intentType)
				{
					case "application/json":
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

								Console.WriteLine(content);
							}

							inputStream.Close();
							inputStream.Dispose();

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





//namespace Kalendarzyk
//{
//	[Activity(Label = "FormSecondApp", Theme = "@style/MainTheme", MainLauncher = true, Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

//	[IntentFilter(new[] { Intent.ActionView, Intent.ActionSend },
//	Categories = new[] { Intent.CategoryDefault },
//	DataMimeType = "application/json")]
//	[IntentFilter(new[] { Intent.ActionView, Intent.ActionSend },
//	Categories = new[] { Intent.CategoryDefault },
//	DataMimeType = "text/calendar")]
//	[IntentFilter(new[] { Intent.ActionView, Intent.ActionSend },
//	Categories = new[] { Intent.CategoryDefault },
//	DataMimeType = "application/vnd.jolovCompany.kics")]
//	public class MainActivity : MauiAppCompatActivity
//	{
//		protected override void OnCreate(Bundle savedInstanceState)
//		{
//			base.OnCreate(savedInstanceState);

//			if (Intent?.Action == Android.Content.Intent.ActionOpenDocument)
//			{
//				Stream? inputStream = null;
//				var filePath = Intent?.ClipData?.GetItemAt(0);
//				if (filePath?.Uri != null)
//				{
//					inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
//				}

//				if (inputStream != null)
//				{
//					using (var reader = new StreamReader(inputStream))
//					{
//						var content = reader.ReadToEnd();

//						// TODO HERE
//					}

//					inputStream.Close();
//					inputStream.Dispose();
//				}
//			}
//		}
//	}
//	//	protected override void OnCreate(Bundle savedInstanceState)
//	//	{
//	//		base.OnCreate(savedInstanceState);

//	//		//// Determine the MIME type of the intent
//	//		//if (Intent?.Action == Android.Content.Intent.ActionOpenDocument)
//	//		//{
//	//		//	var intentType = Intent.Type;
//	//		//	if (intentType != null)
//	//		//	{
//	//		//		HandleIntentAccordingToType(intentType);

//	//		//	}

//	//		//}

//	//	}





//	//	private void HandleIntentAccordingToType(string intentType)
//	//	{
//	//		switch (intentType)
//	//		{
//	//			case "application/json":
//	//				Stream? inputStream = null;
//	//				var filePath = Intent?.ClipData?.GetItemAt(0);
//	//				if (filePath?.Uri != null)
//	//				{
//	//					inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
//	//				}

//	//				if (inputStream != null)
//	//				{
//	//					using (var reader = new StreamReader(inputStream))
//	//					{
//	//						var content = reader.ReadToEnd();

//	//						Console.WriteLine(content);
//	//					}

//	//					inputStream.Close();
//	//					inputStream.Dispose();

//	//				}
//	//				break;
//	//			case "text/calendar":
//	//				string json2 = Intent.GetStringExtra(Intent.ExtraText);
//	//				Console.WriteLine(json2); break;
//	//			case "application/kics":
//	//				string json3 = Intent.GetStringExtra(Intent.ExtraText);
//	//				Console.WriteLine(json3); break;
//	//			default:
//	//				// Handle unknown or unsupported file types
//	//				break;
//	//		}
//	//	}
//	//}
//}








//protected override void OnCreate(Bundle savedInstanceState)
//{
//	base.OnCreate(savedInstanceState);

//	if (Intent?.Action == Android.Content.Intent.ActionOpenDocument)
//	{
//		Stream? inputStream = null;
//		var filePath = Intent?.ClipData?.GetItemAt(0);
//		if (filePath?.Uri != null)
//		{
//			inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
//		}

//		if (inputStream != null)
//		{
//			using (var reader = new StreamReader(inputStream))
//			{
//				var content = reader.ReadToEnd();

//				// TODO HERE
//			}

//			inputStream.Close();
//			inputStream.Dispose();
//		}
//	}
//}