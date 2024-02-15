using Kalendarzyk.Helpers;
using Kalendarzyk.Models;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;
using System.Windows.Input;

namespace Kalendarzyk.Views;

public partial class WelcomePage : ContentPage
{
	public ICommand AddToFavoritesCommand { get; }

	public WelcomePage()
	{
		InitializeComponent();
		BindingContext = this;



		//TODO HERE: AddToFavoritesCommand
/*		AddToFavoritesCommand = new Command<string>((pageType) =>
		{
			// reference Shell instance here to call the method
			(Application.Current.MainPage as AppShell)?.AddDeleteFromFavorites("New Page", Type.GetType(pageType));
		});*/
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
/*		Favourites tab
 *		var pageType = "Kalendarzyk.Views.EventPage";
		(Application.Current.MainPage as AppShell)?.AddDeleteFromFavorites("New Page", Type.GetType(pageType));
*/
	}
}