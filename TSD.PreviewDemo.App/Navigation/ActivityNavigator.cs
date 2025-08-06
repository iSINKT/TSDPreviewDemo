using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TSD.PreviewDemo.App.Utilities;
// ReSharper disable ChangeFieldTypeToSystemThreadingLock

namespace TSD.PreviewDemo.App.Navigation
{
    internal class ActivityNavigator
    {
        private static Dictionary<string, Type> _processStateToActivityMap = new();
        private static ActivityNavigator _instanceActivityNavigator;
        private static readonly object Lock = new();

        private ActivityNavigator()
        {
            lock (Lock)
            {
                _instanceActivityNavigator ??= this;
            }
        }

        public static ActivityNavigator BuildNavigator(Assembly rootAppAssembly)
        {
            if (_instanceActivityNavigator != null)
                return _instanceActivityNavigator;

            var businessProcessStateTypes = rootAppAssembly.GetTypeListMarkedAs<BusinessProcessStateAttribute>().ToList();

            _processStateToActivityMap = new Dictionary<string, Type>();
            foreach (var businessProcessStateType in businessProcessStateTypes)
            {
                var keys = businessProcessStateType.GetTypeInfo().GetCustomAttributes<BusinessProcessStateAttribute>();
                var value = businessProcessStateType;
                foreach (var key in keys)
                    _processStateToActivityMap.Add(key.ProcessState, value);
            }
            return new ActivityNavigator();
        }

        public Intent NavigateTo(string businessProcessState)
        {
            var typeOfBaseActivity = GetActivityType(businessProcessState);
            var intent = new Intent(Application.Context, typeOfBaseActivity);
            return intent;
        }

        private static Type GetActivityType(string processState)
        {
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
            if (!_processStateToActivityMap.ContainsKey(processState))
                throw new ApplicationException($"Невозможно определить форму для состояния бизнес-процесса: {processState}");
            return _processStateToActivityMap[processState];
        }
    }
}
