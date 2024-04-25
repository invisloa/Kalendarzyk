using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public partial class ChangableFontsIconAdapter : ObservableObject
	{
		[ObservableProperty]
		private bool _isSelected;

		[ObservableProperty]
		private RelayCommand _isSelectedCommand;

		[ObservableProperty]
		private string _iconFontText;

		[ObservableProperty]
		private string _selectedIconFontText;
		[ObservableProperty]
		private string _notSelectedIconFontText;
		public ChangableFontsIconAdapter(bool isSelected, string selectedIconText, string notSelectedIconText)
		{
			IsSelected = isSelected;
			SelectedIconFontText = selectedIconText;
			NotSelectedIconFontText = notSelectedIconText;
			IsSelectedCommand = new RelayCommand(OnIsSelectedCommand);
			IconFontText = IsSelected ? SelectedIconFontText : NotSelectedIconFontText;
		}
		private void OnIsSelectedCommand()
		{
			IsSelected = !IsSelected;
			IconFontText = IsSelected ? SelectedIconFontText : NotSelectedIconFontText;
		}
	}
}
