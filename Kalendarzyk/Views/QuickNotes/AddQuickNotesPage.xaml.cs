using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views.QuickNotes;

public partial class AddQuickNotesPage : ContentPage
{
	public AddQuickNotesPage()
	{
		var _eventRepository = ServiceHelper.GetService<IEventRepository>();
		InitializeComponent();
		BindingContext = new AddQuickNotesViewModel(_eventRepository);
	}
	public AddQuickNotesPage(IEventRepository eventRepository, IGeneralEventModel quickNoteToEdit)
	{
		InitializeComponent();
		BindingContext = new AddQuickNotesViewModel(eventRepository, quickNoteToEdit);
	}

}