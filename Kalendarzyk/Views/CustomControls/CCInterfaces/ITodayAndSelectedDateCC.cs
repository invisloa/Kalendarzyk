namespace Kalendarzyk.Views.CustomControls.CCInterfaces
{
	public interface ITodayAndSelectedDateCC
	{
		DateTime CurrentSelectedDate { get; set; }
		DateTime CurrentDate { get; }
		RelayCommand SelectTodayDateCommand { get; set; }
	}
}
