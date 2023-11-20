using Kalendarzyk.ViewModels;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Kalendarzyk.Views;

public partial class TestPageNet8 : ContentPage
{
	public TestPageNet8()
	{
		BindingContext = new TestPageNet8ViewModel();

		InitializeComponent();
	}
}