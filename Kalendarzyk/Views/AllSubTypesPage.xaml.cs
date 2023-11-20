using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.TypesViewModels;
using System.Net.WebSockets;

namespace Kalendarzyk.Views;

public partial class AllSubTypesPage : ContentPage
{
	public AllSubTypesPage()
	{
		var eventRepository = ServiceHelper.GetService<IEventRepository>();
		BindingContext = new AllSubTypesPageViewModel(eventRepository);
		InitializeComponent();
	}
}