using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Swftx.Wpf.ViewModels;

public abstract class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual bool Set<T>(
        ref T field,
        T value,
        Func<T?, bool> Validator,
        [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value) || !Validator(value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual bool Set<T>(
        ref T field,
        T value,
        string? validationErrorMeassage,
        Func<T?, bool> Validator,
        [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return false;
        if (!Validator(value))
            throw new ArgumentException(validationErrorMeassage ?? $"Property {propertyName} data validation error", nameof(value));

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual bool Set<T>(T value, T oldValue, Action<T> Setter, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(value, oldValue)) return false;

        Setter(value);
        OnPropertyChanged();
        return true;
    }

    public SetValueResult<T> SetValue<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return new SetValueResult<T>(false, field, value, this);

        field = value;
        OnPropertyChanged(propertyName);
        return new SetValueResult<T>(true, field, value, this);
    }

    public readonly struct SetValueResult<T>
    {
        private readonly bool _result;
        private readonly T _oldValue;
        private readonly T _newValue;
        private readonly ViewModel _viewModel;

        public SetValueResult(bool result, in T oldValue, in T newValue, ViewModel viewModel)
        {
            _result = result;
            _oldValue = oldValue;
            _newValue = newValue;
            _viewModel = viewModel;
        }

        public SetValueResult<T> UpdateProperty(string propertyName)
        {
            if (_result) _viewModel.OnPropertyChanged(propertyName);

            return this;
        }

        public SetValueResult<T> Then(Action<T> action)
        {
            if (_result) action(_newValue);

            return this;
        }

        public SetValueResult<T> ThenIf(Func<T, bool> valueChecker, Action<T> action)
        {
            if (_result && valueChecker(_newValue)) action(_newValue);

            return this;
        }

        public SetValueResult<T> ThenIf(Func<T, T, bool> valueChecker, Action<T> action)
        {
            if (_result && valueChecker(_oldValue, _newValue)) action(_newValue);

            return this;
        }
    }
}
