using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TSD.PreviewDemo.Common.Extensions;

namespace TSD.PreviewDemo.Common.Utils
{
    public static class CommonReflectionExtensions
    {
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