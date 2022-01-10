using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nulah.PhantomIndex.Core.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        /// <summary>
        /// Updates a property value and raises <see cref="PropertyChanged"/> if the incoming value is different.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="____DO_NOT_SET___propertyName"></param>
        protected void NotifyAndSetPropertyIfChanged<T>(ref T property, T value,
            [CallerMemberName] string ____DO_NOT_SET___propertyName = null)
        {
            VerifyPropertyName(____DO_NOT_SET___propertyName);

            if (value == null || value.Equals(property) == false)
            {
                property = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(____DO_NOT_SET___propertyName));
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event then executes the given action
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="changeNotifyAction"></param>
        protected void OnPropertyChanged(string propertyName, Action changeNotifyAction)
        {
            VerifyPropertyName(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            changeNotifyAction();
        }

        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        private bool _pageEnabled;
        public bool PageEnabled
        {
            get => _pageEnabled;
            set => NotifyAndSetPropertyIfChanged(ref _pageEnabled, value);
        }

        private List<string> _validationErrors;

        public List<string> ValidationErrors
        {
            get => _validationErrors;
            set => NotifyAndSetPropertyIfChanged(ref _validationErrors, value);
        }

        /// <summary>
        /// Validates the view models based on <see cref="System.ComponentModel.DataAnnotations"/> attributes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public bool Validate()
        {
            var ctx = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, ctx, validationResults, true);

            if (!isValid)
            {
                var validationErrors = new List<string>();

                foreach (ValidationResult validationResult in validationResults)
                {
                    validationErrors.Add(validationResult.ErrorMessage);
                }

                ValidationErrors = validationErrors;

                return false;
            }

            ValidationErrors = null;

            return true;
        }
    }
}
