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
/*		TODO JO TO CHECK FAVOURITES TAB
 *		
 *		
 *		//  method when the user wants to add a page to favorites.
		public void AddDeleteFromFavorites(string title, Type pageType)
		{
			var favouritesTab = FindFavouritesTab();
			if (favouritesTab != null)
			{
				this.Items.Remove(favouritesTab);		// TODO JO SOMETIMES IT CRASHES HERE ??

				var shellContent = new ShellContent
				{
					Title = title,
					ContentTemplate = new DataTemplate(pageType),
					Route = title.Replace(" ", string.Empty)
				};

				favouritesTab.Items.Add(shellContent);

				// Use Dispatcher.Dispatch() to ensure UI updates happen on the main thread
				this.Dispatcher.Dispatch(() =>
				{
					this.Items.Add(favouritesTab);
					this.CurrentItem = favouritesTab; // Reselect the tab
				});
			}
		}



		private Tab FindFavouritesTab()
		{
			foreach (var item in this.Items)
			{
				if (item is FlyoutItem flyoutItem)
				{
					foreach (var tab in flyoutItem.Items)
					{
						if (tab is Tab tabItem && tabItem.Title == "Favourites")
						{
							return tabItem;
						}
					}
				}
			}
			return null; // Favourites tab not found
		}
*/	}
}
