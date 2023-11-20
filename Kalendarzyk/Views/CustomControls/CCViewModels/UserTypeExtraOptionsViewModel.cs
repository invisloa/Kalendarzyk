using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.ViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	internal class UserTypeExtraOptionsViewModel : BaseViewModel, IUserTypeExtraOptionsViewModel
	{
		public UserTypeExtraOptionsViewModel(bool isEditMode)
		{
			IsEditMode = isEditMode;
			ValueTypeClickCommand = new RelayCommand(() => IsValueTypeSelected = !IsValueTypeSelected);
			IsMicroTaskListTypeSelectedCommand = new RelayCommand(() => IsMicroTaskTypeSelected = !IsMicroTaskTypeSelected);
			IsDefaultTimespanSelectedCommand = new RelayCommand(() => IsDefaultEventTimespanSelected = !IsDefaultEventTimespanSelected);
		}
		public RelayCommand ValueTypeClickCommand { get; set; }
		public RelayCommand IsMicroTaskListTypeSelectedCommand { get; set; }
		public RelayCommand IsDefaultTimespanSelectedCommand { get; set; }

		private Color _deselectedColor = (Color)Application.Current.Resources["DeselectedBackgroundColor"];
		private bool _isValueTypeSelected;
		private Color _selectedColor = (Color)Application.Current.Resources["MainButtonBackgroundColor"];
		public bool IsValueTypeSelected
		{
			get => _isValueTypeSelected;
			set
			{
				if (_isValueTypeSelected != value)
				{
					_isValueTypeSelected = value;
					OnPropertyChanged(nameof(IsValueTypeSelected));
					OnPropertyChanged(nameof(IsValueTypeColor));
				}
			}
		}
		public bool IsEditMode { get; set; }

		private bool _isMicroTasksTypeSelected;
		public bool IsMicroTaskTypeSelected
		{
			get => _isMicroTasksTypeSelected;
			set
			{
				if (_isMicroTasksTypeSelected != value)
				{
					_isMicroTasksTypeSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsMicroTasksListTypeColor));
				}
			}
		}
		private bool _isDefaultEventTimespanSelected;
		public bool IsDefaultEventTimespanSelected
		{
			get => _isDefaultEventTimespanSelected;
			set
			{
				if (_isDefaultEventTimespanSelected != value)
				{
					_isDefaultEventTimespanSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsDefaultTimespanColor));
				}
			}
		}

		public Color IsValueTypeColor
		{
			get
			{
				if (IsValueTypeSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}

		public Color IsMicroTasksListTypeColor
		{
			get
			{
				if (IsMicroTaskTypeSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
		public Color IsDefaultTimespanColor
		{
			get
			{
				if (IsDefaultEventTimespanSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}

		public bool IsNotEditMode => !IsEditMode;
	}
}
