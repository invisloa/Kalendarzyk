using System.Collections;

namespace Kalendarzyk.Views.CustomControls
{
	public partial class SelectableButtonFlexLayoutCC : FlexLayout
	{
		private static Color _mainButtonBackgroundColor;

		public SelectableButtonFlexLayoutCC()
		{
			_mainButtonBackgroundColor = (Color)Application.Current.Resources["MainButtonBackgroundColor"];
			InitializeComponent();
		}

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SelectableButtonFlexLayoutCC), null, propertyChanged: OnItemsSourceChanged);


		public static readonly BindableProperty ButtonBackgroundProperty =
			BindableProperty.Create(nameof(ButtonBackground), typeof(Color), typeof(SelectableButtonFlexLayoutCC), defaultValue: Colors.Black);

		public Color ButtonBackground
		{
			get => (Color)GetValue(ButtonBackgroundProperty);
			set => SetValue(ButtonBackgroundProperty, value);
		}

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SelectableButtonFlexLayoutCC control && newValue is IEnumerable items)
			{
				BindableLayout.SetItemsSource(control, items);
			}
		}
	}
}
