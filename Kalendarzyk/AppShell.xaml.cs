using Kalendarzyk.Views;

namespace Kalendarzyk
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();


			// below is from some tutorial check if it is needed
			// TODO JO TO CHECK
			// Register routes
			Routing.RegisterRoute("eventpage", typeof(EventPage));
		}
	}
}
