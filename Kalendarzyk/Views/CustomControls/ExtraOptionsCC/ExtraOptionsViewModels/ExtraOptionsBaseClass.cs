using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.ViewModels;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalendarzyk.Services;


namespace Kalendarzyk.Views.CustomControls.ExtraOptionsCC.ExtraOptionsViewModels
{
    public partial class  ExtraOptionsBaseClass : ObservableObject
	{
		private bool _isMicroTasksType;
		public bool IsMicroTasksType
		{
			get => _isMicroTasksType;
			set
			{
				SetProperty(ref _isMicroTasksType, value);
				if (!value)
				{
					IsMicroTasksBtnSelected = false;
				}
			}
		}
		private bool _isValueType;
		public bool IsValueType
		{
			get => _isValueType;
			set
			{
				SetProperty(ref _isValueType, value);
				if (!value)
				{
					IsValueBtnSelected = false;
				}
			}
		}
		private bool _isMicroTasksBtnSelected;
		public bool IsMicroTasksBtnSelected
		{
			get
			{
				if (IsMicroTasksType)
				{
					return _isMicroTasksBtnSelected;
				}
				else
				{
					return false;
				}
			}
			set => SetProperty(ref _isMicroTasksBtnSelected, value);
		}
		private bool _isValueBtnSelected;
		public bool IsValueBtnSelected
		{
			get
			{
				if (IsValueType)
				{
					return _isValueBtnSelected;
				}
				else
				{
					return false;
				}
			}
			set => SetProperty(ref _isValueBtnSelected, value);
		}

		[ObservableProperty]
		private MicroTasksCCAdapterVM _microTasksCCAdapter;
		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();

		[ObservableProperty]
		private ChangableFontsIconAdapter _changableFontsIconCC;
		private bool _isCompleted;
		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _extraOptionsButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>();
		[ObservableProperty]
		private ISubEventTypeModel _subEventType;


		protected void OnIsMicroTasksSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsMicroTasksBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		protected void OnIsEventValueTypeCommand(SelectableButtonViewModel clickedButton)
		{
			IsValueBtnSelected = !clickedButton.IsSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		protected void SetPropperValueType()
		{
			DefaultMeasurementSelectorCCHelper.SelectPropperMeasurementData(SubEventType);
		}
	}
}
