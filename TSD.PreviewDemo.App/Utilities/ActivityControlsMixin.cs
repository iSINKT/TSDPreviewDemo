using Android.App;
using Android.Views;
using System;

namespace TSD.PreviewDemo.App.Utilities
{
    public static class ActivityControlsMixin
    {
        public static void AttachSubscription<TAttr, T>(this Activity activity, IObservable<T> observable,
            Func<View, Action<T>> action) where TAttr : Attribute
        {
            var controlsWithAttribute = ReflectionExtensions.GetViewsWithAttribute<TAttr>(activity);

            foreach (var control in controlsWithAttribute)
            {
                observable.Subscribe(action(control));
            }
        }

        public static void RunOnceAction<TAttr>(this Activity activity, Action<View> action) where TAttr : Attribute
        {
            var controlsWithAttribute = ReflectionExtensions.GetViewsWithAttribute<TAttr>(activity);

            foreach (var control in controlsWithAttribute)
            {
                 action.Invoke(control);
            }
        }
    }
}
