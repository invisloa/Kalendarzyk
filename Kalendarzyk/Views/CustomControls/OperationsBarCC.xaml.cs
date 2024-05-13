using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls;
public partial class OperationsBarCC : ContentView
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public static readonly BindableProperty IsEventTypesButtonisibleProperty = BindableProperty.Create(
		nameof(IsEventTypesButtonisible),
		typeof(bool),
		typeof(OperationsBarCC),
		false,
		propertyChanged: OnIsEventTypesButtonisiblePropertyChanged);

	private static void OnIsEventTypesButtonisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (OperationsBarCC)bindable;
		control.OnPropertyChanged(nameof(IsEventTypesButtonisible));
	}
	public bool IsEventTypesButtonisible
	{
		get => (bool)GetValue(IsEventTypesButtonisibleProperty);
		set => SetValue(IsEventTypesButtonisibleProperty, value);
	}

	public static readonly BindableProperty IsCompletedButtonVisibleProperty = BindableProperty.Create(
	nameof(IsCompletedButtonVisible),
	typeof(bool),
	typeof(OperationsBarCC),
	false,
	propertyChanged: OnIsCompletedButtonisiblePropertyChanged);

	private static void OnIsCompletedButtonisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (OperationsBarCC)bindable;
		control.OnPropertyChanged(nameof(IsCompletedButtonVisible));
	}
	public bool IsCompletedButtonVisible
	{
		get => (bool)GetValue(IsEventTypesButtonisibleProperty);
		set => SetValue(IsEventTypesButtonisibleProperty, value);
	}


	public static readonly BindableProperty IsDeleteButtonVisibleProperty = BindableProperty.Create(
		nameof(IsDeleteButtonVisible),
		typeof(bool),
		typeof(OperationsBarCC),
		false,
		propertyChanged: OnIsDeleteButtonVisiblePropertyChanged);

	private static void OnIsDeleteButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (OperationsBarCC)bindable;
		control.OnPropertyChanged(nameof(IsDeleteButtonVisible));
	}
	public bool IsDeleteButtonVisible
	{
		get => (bool)GetValue(IsDeleteButtonVisibleProperty);
		set => SetValue(IsDeleteButtonVisibleProperty, value);
	}
	public static readonly BindableProperty AsyncDeleteButtonCommandProperty = BindableProperty.Create(nameof(AsyncDeleteButtonCommand), typeof(System.Windows.Input.ICommand), typeof(OperationsBarCC), null);
	public ICommand AsyncDeleteButtonCommand
	{
		get => (ICommand)GetValue(AsyncDeleteButtonCommandProperty);
		set => SetValue(AsyncDeleteButtonCommandProperty, value);
	}


	public static readonly BindableProperty IsShareButtonVisibleProperty = BindableProperty.Create(nameof(IsShareButtonVisible), typeof(bool), typeof(OperationsBarCC), false);
	public bool IsShareButtonVisible
	{
		get => (bool)GetValue(IsShareButtonVisibleProperty);
		set => SetValue(IsShareButtonVisibleProperty, value);
	}
	public static readonly BindableProperty AsyncShareEventButtonCommandProperty = BindableProperty.Create(nameof(AsyncShareEventButtonCommand), typeof(System.Windows.Input.ICommand), typeof(OperationsBarCC), null);
	public ICommand AsyncShareEventButtonCommand
	{
		get => (ICommand)GetValue(AsyncShareEventButtonCommandProperty);
		set => SetValue(AsyncShareEventButtonCommandProperty, value);
	}

	public static readonly BindableProperty IsSaveButtonClickableProperty = BindableProperty.Create(nameof(IsSaveButtonClickable), typeof(bool), typeof(OperationsBarCC), false);
	public bool IsSaveButtonClickable
	{
		get => (bool)GetValue(IsSaveButtonClickableProperty);
		set => SetValue(IsSaveButtonClickableProperty, value);
	}
	public static readonly BindableProperty AsyncSaveButtonCommandProperty = BindableProperty.Create(nameof(AsyncSaveButtonCommand), typeof(System.Windows.Input.ICommand), typeof(OperationsBarCC), null);
	public ICommand AsyncSaveButtonCommand
	{
		get => (ICommand)GetValue(AsyncSaveButtonCommandProperty);
		set => SetValue(AsyncSaveButtonCommandProperty, value);
	}


	public OperationsBarCC()
	{
		InitializeComponent();
	}
}