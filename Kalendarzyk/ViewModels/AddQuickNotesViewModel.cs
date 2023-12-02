using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Helpers;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.ViewModels
{
	public partial class AddQuickNotesViewModel : ObservableObject
	{
		public ObservableCollection<SelectableButtonViewModel> QuickNoteLabelColors { get; set; }
		public RelayCommand<SelectableButtonViewModel> QuickNoteLabelColorSeletctionCommand => new RelayCommand<SelectableButtonViewModel>(OnQuickNoteLabelColorSeletctionCommand);
		

		[ObservableProperty]
		private Color _quickNoteLabelColor=  Colors.Red;

		[ObservableProperty]
		private bool _isQuickNoteLabelColors;

		[ObservableProperty]
		private string _submitQuickNoteButtonText = "ADD QUICK NOTE";

		[ObservableProperty]
		private bool _isQuickNoteSubTypeSelected;
		[ObservableProperty]
		private bool _isQuickNotDatesSelected;

		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors;
		public AddQuickNotesViewModel()
        {
			QuickNoteLabelColors = SelectableButtonHelper.GenerateColorPaletteButtons();
			InitializeButtonSelectors();

		}

		private void InitializeButtonSelectors()
		{
			QuickNotesButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("COLOR", false, new RelayCommand<SelectableButtonViewModel>(OnIsColorsSelectedCommand)),
				new SelectableButtonViewModel("SUBTYPE", false, new RelayCommand<SelectableButtonViewModel>(OnIsShowSubTypesCommand)),
				new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnISDatesControlsCommand)),
			};
			//InitializeIconsTabs();
		}
		private void OnIsColorsSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteLabelColors = !IsQuickNoteLabelColors;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsShowSubTypesCommand(SelectableButtonViewModel clickedButton)
		{
			_isQuickNoteSubTypeSelected = !_isQuickNoteSubTypeSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnISDatesControlsCommand(SelectableButtonViewModel clickedButton)
		{
			_isQuickNotDatesSelected = !_isQuickNotDatesSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}

		private void OnQuickNoteLabelColorSeletctionCommand(SelectableButtonViewModel clickedButton)
		{
			QuickNoteLabelColor = clickedButton.ButtonColor;
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, QuickNoteLabelColors);

		}




	}
}
