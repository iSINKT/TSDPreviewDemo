using Android.Views;
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.App.Utilities
{
    public static class ViewControlsHelper
    {
        /// <summary>
        /// Make Buttons disabled or enabled. Can be used when we wait response from server, and we need to lock buttons
        /// </summary>
        /// <param name="enableType"></param>
        /// <param name="buttons"></param>
        public static void ControlButtons(EnableType enableType, params View[] buttons)
        {
            if (enableType == EnableType.Enable)
            {
                foreach (var button in buttons)
                {
                    button.Enabled = true;
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    button.Enabled = false;
                }
            }
        }

        public enum EnableType : byte
        {
            Enable,
            Disable
        }
    }
}