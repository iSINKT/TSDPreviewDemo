using Android.App;
using Android.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TSD.PreviewDemo.Common.Extensions;

namespace TSD.PreviewDemo.App.Utilities
{
    internal static class ReflectionExtensions
    {
        internal static string GetResourceName(this PropertyInfo member)
        {
            var resourceNameOverride = member.CustomAttributes
                .Select(attribute => attribute.ConstructorArguments.Select(argument => argument.Value)).FirstOrDefault()!.FirstOrDefault()
                ?.ToString();
            return resourceNameOverride ?? member.Name;
        }

        internal static IEnumerable<View> GetViewsWithAttribute<TAttr>(Activity activity) where TAttr : Attribute
        {
            var members = activity.GetType().GetProperties();

            var propertyInfoWithAttribute = members.Where(member => member.GetCustomAttribute<TAttr>(true) != null).ToList();
            foreach (var member in propertyInfoWithAttribute)
            {
                var view = activity.GetControl(member.GetResourceName());
                member.SetValue(activity, view);
            }
            return propertyInfoWithAttribute.Select(propertyInfo => (View)propertyInfo.GetValue(activity));
        }

        /// <summary>
        /// Extension method which get all types with NavigationTracked attribute to fill ProcessStateToActivityMap
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeListMarkedAs<T>(this Assembly assembly) where T : Attribute
        {
            assembly.ThrowIfNull(nameof(assembly));
            return assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(T)));
        }
    }
}