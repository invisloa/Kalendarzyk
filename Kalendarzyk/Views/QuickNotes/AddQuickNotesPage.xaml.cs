using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Views.QuickNotes;

public partial class AddQuickNotesPage : ContentPage
{
	public AddQuickNotesPage()
	{
		InitializeComponent();
		BindingContext = new AddQuickNotesViewModel();
	}
	public AddQuickNotesPage(IEventRepository eventRepository, IGeneralEventModel quickNoteToEdit)
	{
		InitializeComponent();
		BindingContext = new AddQuickNotesViewModel(quickNoteToEdit);
	}
	protected override bool OnBackButtonPressed()
	{
		if (!((AddQuickNotesViewModel)BindingContext).IsModified)
		{
			return base.OnBackButtonPressed();
		}
		else
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				var result = await this.DisplayAlert("Question?", "Do You want to save the changes", "Yes", "No");
				if (result)
				{
					await ((AddQuickNotesViewModel)BindingContext).AsyncSubmitQuickNoteCommand.ExecuteAsync(null);
				}
				else
				{
					await Navigation.PopAsync();
				}
			});
			return true;
		}
	}
}