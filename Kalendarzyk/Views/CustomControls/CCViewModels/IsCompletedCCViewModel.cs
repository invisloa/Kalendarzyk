using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
