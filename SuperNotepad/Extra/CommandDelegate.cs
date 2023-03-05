using System;
using System.Windows.Input;

namespace SuperNotepad.Extra;

public class CommandDelegate : ICommand {
    private readonly Action<object?>? _execute;
    private readonly Func<object?, bool>? _canExecute;

    public CommandDelegate(Action<object?>? execute, Func<object?, bool>? canExecute = null) {
        this._execute = execute;
        this._canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object? parameter) {
        _execute?.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}