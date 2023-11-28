using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	class ExpandableCCViewModel : BindableObject
	{
		private bool _isExpanded;

		public bool IsExpanded
		{
			get => _isExpanded;
			set
			{
				_isExpanded = value;
				OnPropertyChanged();
			}
		}

		public ICommand ToggleExpandCommand { get; }

		public ExpandableCCViewModel()
		{
			ToggleExpandCommand = new Command(ToggleExpand);
		}

		private void ToggleExpand()
		{
			IsExpanded = !IsExpanded;
		}
	}
}