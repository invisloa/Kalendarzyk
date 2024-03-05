using CommunityToolkit.Mvvm.ComponentModel;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public partial class IsCompletedCCViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool _isCompleted;

		[ObservableProperty]
		private RelayCommand _isCompleteFrameCommand;

		[ObservableProperty]
		private string _isCompleteIconFontText;


		public IsCompletedCCViewModel(bool isCompleted)
		{
			IsCompleted = isCompleted;
			IsCompleteFrameCommand = new RelayCommand(OnIsCompleteFrameCommand);
			IsCompleteIconFontText = IsCompleted ? "check_box" : "check_box_outline_blank";
		}
		private void OnIsCompleteFrameCommand()
		{
			IsCompleted = !IsCompleted;
			IsCompleteIconFontText = IsCompleted ? "check_box" : "check_box_outline_blank";
		}
	}
}
