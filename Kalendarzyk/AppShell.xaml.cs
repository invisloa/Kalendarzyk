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
		// This method can be called when the user wants to add a page to favorites.
		public void AddDeleteFromFavorites(string title, Type pageType)
		{
			var tab = this.Items.FirstOrDefault(item => item.CurrentItem.Title == "Favourites");
			if (tab != null)
			{
				var shellContent = new ShellContent
				{
					Title = title,
					ContentTemplate = new DataTemplate(pageType)
				};

				tab.Items.Add(shellContent);
			}
		}
	}
}
