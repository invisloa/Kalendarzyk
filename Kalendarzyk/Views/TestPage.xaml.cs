namespace Kalendarzyk.Views;
using Plugin.LocalNotification;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		var request = new NotificationRequest
		{
			Title = "Test",
			Subtitle = "TestS",
			Description = "Test",
			CategoryType = NotificationCategoryType.Alarm,
			Schedule = new NotificationRequestSchedule
			{
				NotifyTime = DateTime.Now.AddSeconds(27)
			}

		}; var request2 = new NotificationRequest
		{
			Title = "Tes2",
			Subtitle = "TestS",
			Description = "Test",
			CategoryType = NotificationCategoryType.Alarm,
			Schedule = new NotificationRequestSchedule
			{
				NotifyTime = DateTime.Now.AddSeconds(25)
			}

		};
		await LocalNotificationCenter.Current.Show(request);
		await LocalNotificationCenter.Current.Show(request2);
	}
}