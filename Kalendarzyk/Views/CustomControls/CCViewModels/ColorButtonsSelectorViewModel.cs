using Kalendarzyk.Helpers;
using Kalendarzyk.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public class ColorButtonsSelectorViewModel : BaseViewModel
	{
		private Color _selectedColor;
		private ObservableCollection<SelectableButtonViewModel> _colorButtons;

		public ObservableCollection<SelectableButtonViewModel> ColorButtons
		{
			get => _colorButtons;
			set
			{
				_colorButtons = value;
				OnPropertyChanged();
			}
		}

		public Color SelectedColor
		{
			get => _selectedColor;
			set
			{
				_selectedColor = value;
				OnPropertyChanged();
				UpdateButtonSelection();
			}
		}

		public ICommand SelectedButtonCommand { get; private set; }

		// Primary constructor
		public ColorButtonsSelectorViewModel(
			ObservableCollection<SelectableButtonViewModel> colorButtons = null,
			ICommand selectedButtonCommand = null,
			Color? startingColor = null)
		{
			ColorButtons = colorButtons ?? SelectableButtonHelper.GenerateColorPaletteButtons();
			SelectedButtonCommand = selectedButtonCommand ?? new Command<SelectableButtonViewModel>(OnColorSelectionCommand);
			SelectedColor = startingColor ?? Colors.Red;
		}

		private void OnColorSelectionCommand(SelectableButtonViewModel clickedButton)
		{
			SelectedColor = clickedButton.ButtonColor;
		}

		private void UpdateButtonSelection()
		{
			foreach (var button in ColorButtons)
			{
				button.IsSelected = button.ButtonColor == SelectedColor;
			}
		}
	}
}
