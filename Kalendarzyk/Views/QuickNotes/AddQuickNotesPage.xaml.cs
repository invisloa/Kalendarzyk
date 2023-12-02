using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views.QuickNotes;

public partial class AddQuickNotesPage : ContentPage
{
	public AddQuickNotesPage()
	{
		InitializeComponent();
		BindingContext = new AddQuickNotesViewModel();
	}
}