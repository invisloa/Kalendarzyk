using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Services.EventsSharing;
using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.EventsViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace Kalendarzyk
{
	public static class MauiProgram
	{
		private const string DefaultProgramName = "Kalendarzyk";
		private const string DefaultJsonEventsFileName = "CalendarEventsD";
		private const string DefaultJsonSubTypesFileName = "CalendarTypesOfEventsD";
		private const string DefaultJsonMainTypesFileName = "CalendarMainTypesOfEventsD";

		//statc mauiapp instance to use it for creating DI
		public static MauiApp Current { get; private set; }


		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()

				.UseMauiCommunityToolkit()
				.UseLocalNotification()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
				});

			// Interfaces DI Dependency Injection for events repository
			builder.Services.AddScoped<IShareEventsService, ShareEventsJson>();

			// ViewModels register
			// AddSingleton - one instance for all timne
			// AddTransient - new instance every time
			builder.Services.AddTransient<AddNewSubTypePageViewModel>();
			builder.Services.AddTransient<AddNewMainTypePageViewModel>();
			builder.Services.AddTransient<MonthlyEventsViewModel>();
			builder.Services.AddTransient<WeeklyEventsViewModel>();
			builder.Services.AddTransient<DailyEventsViewModel>();
			builder.Services.AddTransient<AllEventsViewModel>();
			builder.Services.AddTransient<ValueTypeCalculationsViewModel>();


			// add event dictionary factories DI
			//builder.Services.AddSingleton(eventFactories);

			// Preferences Setting General Properties
			Preferences.Default.Set("ProgramName", DefaultProgramName);
			Preferences.Default.Set("JsonEventsFileName", DefaultJsonEventsFileName);
			Preferences.Default.Set("JsonSubTypesFileName", DefaultJsonSubTypesFileName);
			Preferences.Default.Set("JsonMainTypesFileName", DefaultJsonMainTypesFileName);


#if DEBUG
			builder.Logging.AddDebug();
#endif

			//statc mauiapp instance to use it for creating DI
			Current = builder.Build();

			return Current;
		}
	}
}
