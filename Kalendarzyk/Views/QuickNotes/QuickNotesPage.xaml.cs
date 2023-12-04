using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.QuickNotes;

namespace Kalendarzyk.Views.QuickNotes;

public partial class QuickNotesPage : ContentPage
{
	public QuickNotesPage()
	{
		var _eventRepository = ServiceHelper.GetService<IEventRepository>();
		InitializeComponent();
		BindingContext = new QuickNotesViewModel(_eventRepository);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		
		//BindingContext = new QuickNotesViewModel(_eventRepository);
	}
}