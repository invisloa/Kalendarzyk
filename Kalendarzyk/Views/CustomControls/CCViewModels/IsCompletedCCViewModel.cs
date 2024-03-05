using CommunityToolkit.Mvvm.ComponentModel;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public partial class IsCompletedCCViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool _isCompleted;

		[ObservableProperty]
		private RelayCommand _isCompleteFrameCommand;


		public IsCompletedCCViewModel()
		{
			IsCompleteFrameCommand = new RelayCommand(() => IsCompleted = !IsCompleted);
		}
	}
}
