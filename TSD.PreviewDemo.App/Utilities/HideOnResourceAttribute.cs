using System;

namespace TSD.PreviewDemo.App.Utilities
{
    [AttributeUsage(AttributeTargets.All)]
    public class HideOnResourceAttribute : Attribute
    {
        public HideOnResourceAttribute() { }

        public HideOnResourceAttribute(string resourceName) => ResourceNameOverride = resourceName;

        public string ResourceNameOverride { get; }
    }
}