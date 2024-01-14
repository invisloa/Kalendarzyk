using Kalendarzyk.Services;
using Kalendarzyk.Services.EventsSharing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Kalendarzyk.WinUI
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : MauiWinUIApplication
	{
		IShareEventsService _shareEventsService = Factory.CreateNewShareEventsService();
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
		}

		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

		protected override async void OnLaunched(LaunchActivatedEventArgs args)
		{

			base.OnLaunched(args);

			AppActivationArguments appActivationArguments = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

			if (appActivationArguments != null)
			{
				if (appActivationArguments.Data is Windows.ApplicationModel.Activation.FileActivatedEventArgs fileArgs && fileArgs.Files.Count > 0)
				{
					await HandleFileOpenAsync(fileArgs);
				}
			}
		}
		private async Task HandleFileOpenAsync(Windows.ApplicationModel.Activation.FileActivatedEventArgs fileArgs)
		{

			// Assuming the file is the first one in the list
			var file = fileArgs.Files[0] as Windows.Storage.StorageFile;
			if (file != null)
			{
				// Read the file's content
				string fileContent = await Windows.Storage.FileIO.ReadTextAsync(file);

				await _shareEventsService.ImportEventsAsync(fileContent);
			}
		}
	}

}
