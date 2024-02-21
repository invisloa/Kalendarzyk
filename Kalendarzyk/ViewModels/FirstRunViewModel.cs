using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Services;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kalendarzyk.ViewModels
{
	
	internal partial class FirstRunViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isNextButtonEnabled = false;	// commands canexecute doesnt work with custom buttons
		[ObservableProperty]
		private double englishFlagScale = 1;
		[ObservableProperty]
		private double polishFlagScale = 1;
		[ObservableProperty]
		private string nextButtonText = "NEXT / DALEJ";

		public ICommand EngCommand => new RelayCommand(() => SetSelectedLanguage(0));
		public ICommand PLCommand => new RelayCommand(() => SetSelectedLanguage(1));

		private void SetSelectedLanguage(int language)
		{
			PreferencesManager.SetSelectedLanguage(language);
			SetLaguageVisibility();
		}
		public FirstRunViewModel()
		{

		}
		private void SetLaguageVisibility()
		{
			IsNextButtonEnabled = true;
			if (PreferencesManager.GetSelectedLanguage() == 0)
			{
				EnglishFlagScale = 1.2;
				PolishFlagScale = 1;
				NextButtonText = "NEXT";
			}
			else
			{
				EnglishFlagScale = 1;
				PolishFlagScale = 1.2;
				NextButtonText = "DALEJ";
			}

		}
	}
}
