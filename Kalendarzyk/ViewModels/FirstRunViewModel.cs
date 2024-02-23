using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Models;
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
		private const double smallFlagScale = 0.7;
		private const double bigFlagScale = 1;
		[ObservableProperty]
		private bool isFirstLaunch = Preferences.Default.Get("FirstLaunch", true);
		[ObservableProperty]
		private bool isNextButtonEnabled = false;	// commands canexecute doesnt work with custom buttons
		[ObservableProperty]
		private double englishFlagScale = smallFlagScale;
		[ObservableProperty]
		private double polishFlagScale = smallFlagScale;
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
			Preferences.Default.Set("FirstLaunch", false);

			if ((Enums.LanguageEnum)PreferencesManager.GetSelectedLanguage() == Enums.LanguageEnum.English)
			{
				EnglishFlagScale = bigFlagScale;
				PolishFlagScale = smallFlagScale;
				NextButtonText = "NEXT";
			}
			else
			{
				EnglishFlagScale = smallFlagScale;
				PolishFlagScale = bigFlagScale;
				NextButtonText = "DALEJ";
			}
		}
	}
}
