using Kalendarzyk.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public interface IColorButtonsSelectorHelperClass
	{
		ObservableCollection<SelectableButtonViewModel> ColorButtons { get; set; }
		ICommand SelectedButtonCommand { get; }
		Color SelectedColor { get; set; }
	}
}