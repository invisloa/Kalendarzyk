using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kalendarzyk.ViewModels
{
	internal class TestPageNet8ViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<SomeTestClass> _someTestCollection;

		public ObservableCollection<SomeTestClass> SomeTestCollection
		{
			get => _someTestCollection;
			set
			{
				if (_someTestCollection != value)
				{
					_someTestCollection = value;
					OnPropertyChanged();
				}
			}
		}

		public TestPageNet8ViewModel()
		{
			SomeTestCollection = new ObservableCollection<SomeTestClass>()
		{
			new SomeTestClass() { TestColor = Colors.Red },
			new SomeTestClass() { TestColor = Colors.Blue }
		};
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	public class SomeTestClass : INotifyPropertyChanged
	{
		private Color _testColor;

		public Color TestColor
		{
			get => _testColor;
			set
			{
				if (_testColor != value)
				{
					_testColor = value;
					OnPropertyChanged();
				}
			}
		}

		public SomeTestClass()
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
