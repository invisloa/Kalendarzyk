using Kalendarzyk.Helpers;
using Kalendarzyk.Models;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views;

public partial class WelcomePage : ContentPage
{
	IEventRepository _eventRepository;

	public WelcomePage()
	{
		InitializeComponent();
	}

}