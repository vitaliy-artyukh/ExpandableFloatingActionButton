using System;
using Xamarin.Forms;

namespace ExpandableFloatingActionButton.Utils
{
    public delegate void BindablePropertyChangedHandler<TProperty, TParent>(TParent parent, TProperty oldValue, TProperty newValue);

    public delegate void BindablePropertyChangedHandler<TProperty>(TProperty oldValue, TProperty newValue);

    public delegate bool ValidateValuePredicate<TProperty>(TProperty value);

    public static class BindablePropertyUtils
    {
        public static BindableProperty Create<TProperty, TParent>(string propertyName,
            Func<TParent, BindablePropertyChangedHandler<TProperty>> onPropertyChangedHandlerGetter = null,
            TProperty defaultValue = default,
            BindingMode defaultBindingMode = BindingMode.OneWay) where TParent : BindableObject
        {
            void OnPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
            {
                var parent = (TParent) bindableObject;
                var parentOnPropertyChangedHandler = onPropertyChangedHandlerGetter?.Invoke(parent);
                var castedOldValue = (TProperty) oldValue;
                var castedNewValue = (TProperty) newValue;

                parentOnPropertyChangedHandler?.Invoke(castedOldValue, castedNewValue);
            }

            var hasOnPropertyChangedHandler = onPropertyChangedHandlerGetter != null;
            var onPropertyChangedHandler = hasOnPropertyChangedHandler ? new BindableProperty.BindingPropertyChangedDelegate(OnPropertyChanged) : null;

            var prop = BindableProperty.Create(propertyName,
                typeof(TProperty),
                typeof(TParent),
                defaultValue,
                defaultBindingMode,
                null,
                onPropertyChangedHandler);
            return prop;
        }

        public static BindableProperty CreateAttached<TProperty, TParent>(string propertyName,
            BindablePropertyChangedHandler<TProperty, TParent> onPropertyChangedHandlerGetter = null,
            TProperty defaultValue = default,
            BindingMode defaultBindingMode = BindingMode.OneWay) where TParent : BindableObject
        {
            void OnPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
            {
                var parent = (TParent) bindableObject;
                var castedOldValue = (TProperty) oldValue;
                var castedNewValue = (TProperty) newValue;

                onPropertyChangedHandlerGetter?.Invoke(parent, castedOldValue, castedNewValue);
            }

            var hasOnPropertyChangedHandler = onPropertyChangedHandlerGetter != null;
            var onPropertyChangedHandler = hasOnPropertyChangedHandler ? new BindableProperty.BindingPropertyChangedDelegate(OnPropertyChanged) : null;

            var prop = BindableProperty.CreateAttached(propertyName,
                typeof(TProperty),
                typeof(TParent),
                defaultValue,
                defaultBindingMode,
                propertyChanged: onPropertyChangedHandler);
            return prop;
        }
    }
}