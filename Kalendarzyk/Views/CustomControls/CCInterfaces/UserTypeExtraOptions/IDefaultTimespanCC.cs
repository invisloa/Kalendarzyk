namespace Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions
{
	public interface IDefaultTimespanCC
	{
		// Properties
		int SelectedUnitIndex { get; set; }
		double DurationValue { get; set; }
		void SetControlsValues(TimeSpan timeToAdjust);
		TimeSpan GetDefaultDuration();
	}
}
