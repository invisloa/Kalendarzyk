namespace Kalendarzyk.Views.CustomControls;

public partial class IsNotificationCC : ContentView
{
	public IsNotificationCC()
	{
		InitializeComponent();
		BindingContext = new IsNotificationCCViewModel();
	}
}