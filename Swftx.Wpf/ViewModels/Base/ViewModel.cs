using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Swftx.Wpf.ViewModels
{
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
            if(Equals(value, oldValue)) return false;

            Setter(value);
            OnPropertyChanged(); 
            return true;
        }
    }
}
