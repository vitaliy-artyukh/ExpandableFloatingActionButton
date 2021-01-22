using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ExpandableFloatingActionButton.Extensions
{
    public static class BindableObjectExtensions
    {
        private static readonly Dictionary<Type, Dictionary<string, BindableProperty>> _cachedBindableProperties = new Dictionary<Type, Dictionary<string, BindableProperty>>();

        public static T GetProperty<T>(this BindableObject bindableObject, [CallerMemberName] string propertyName = null)
        {
            var propertyFieldName = $"{propertyName}Property";
            var propertyType = bindableObject.GetType();
            var propertyField = GetBindablePropertyField(propertyType, propertyFieldName);

            var propertyValue = (T) bindableObject.GetValue(propertyField);
            return propertyValue;
        }

        public static void SetProperty<T>(this BindableObject bindableObject, T value, [CallerMemberName] string propertyName = null)
        {
            var propertyFieldName = $"{propertyName}Property";
            var propertyType = bindableObject.GetType();
            var propertyField = GetBindablePropertyField(propertyType, propertyFieldName);

            bindableObject.SetValue(propertyField, value);
        }

        private static BindableProperty GetBindablePropertyField(Type bindableObjectType, string bindablePropertyFieldName)
        {
            if (!_cachedBindableProperties.ContainsKey(bindableObjectType))
            {
                _cachedBindableProperties[bindableObjectType] = new Dictionary<string, BindableProperty>();
            }

            var typePropertiesDictionary = _cachedBindableProperties[bindableObjectType];
            if (!typePropertiesDictionary.ContainsKey(bindablePropertyFieldName))
            {
                var propertyFieldBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
                var propertyFieldInfo = bindableObjectType.GetField(bindablePropertyFieldName, propertyFieldBindingFlags);
                var propertyField = (BindableProperty) propertyFieldInfo.GetValue(null);

                typePropertiesDictionary[bindablePropertyFieldName] = propertyField;
            }

            return typePropertiesDictionary[bindablePropertyFieldName];
        }
    }
}