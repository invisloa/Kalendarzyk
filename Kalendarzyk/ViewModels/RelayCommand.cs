using System;
using System.Windows.Input;

// Generic relay command implementation
public class RelayCommand : ICommand
{
	#region Fields

	private readonly Action _execute;
	private readonly Func<bool> _canExecute;
	private event EventHandler canExecuteChanged;

	#endregion

	#region Constructors

	public RelayCommand(Action execute, Func<bool> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	#endregion

	#region ICommand Members

	public bool CanExecute(object parameter)
	{
		return _canExecute == null || _canExecute();
	}

	public void Execute(object parameter)
	{
		_execute();
	}

	public event EventHandler CanExecuteChanged
	{
		add { canExecuteChanged += value; }
		remove { canExecuteChanged -= value; }
	}

	#endregion

	#region Helper Methods

	public void NotifyCanExecuteChanged()
	{
		canExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
	public void RaiseCanExecuteChanged()
	{
		canExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
	#endregion
}

// Generic relay command implementation with a parameter of type T
public class RelayCommand<T> : ICommand
{
	#region Fields

	private readonly Action<T> _execute;
	private readonly Predicate<T> _canExecute;
	private event EventHandler canExecuteChanged;

	#endregion

	#region Constructors

	public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	#endregion

	#region ICommand Members

	public bool CanExecute(object parameter)
	{
		return _canExecute == null || _canExecute((T)parameter);
	}

	public void Execute(object parameter)
	{
		_execute((T)parameter);
	}

	public event EventHandler CanExecuteChanged
	{
		add { canExecuteChanged += value; }
		remove { canExecuteChanged -= value; }
	}

	#endregion

	#region Helper Methods

	public void RaiseCanExecuteChanged()
	{
		canExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	#endregion
}
