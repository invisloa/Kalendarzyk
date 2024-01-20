using Kalendarzyk.Helpers;
using Kalendarzyk.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{

    //TODO extract code from addneweventviewmodel to this class
	public class ColorButtonsSelectorViewModel : BaseViewModel
	{
        private ObservableCollection<SelectableButtonViewModel> _colorButtons;
        public ObservableCollection<SelectableButtonViewModel> ColorButtons
        {
			get { return _colorButtons; }
			set 
            {
              _colorButtons = value;
              OnPropertyChanged();
            }
		}
        public ColorButtonsSelectorViewModel()
        {
            ColorButtons = SelectableButtonHelper.GenerateColorPaletteButtons();
        }
    }
}
