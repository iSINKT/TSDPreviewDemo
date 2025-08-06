using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
// ReSharper disable All

namespace TSD.PreviewDemo.App.Utilities
{
    public class CustomAdapter<T> : ArrayAdapter<T> 
    {
        public int SelectedPosition;

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            var v = base.GetDropDownView(position, null, parent);
            v?.SetBackgroundColor(position == SelectedPosition ? Color.ParseColor("#557166") : Color.White);
            return v;
        }

        public CustomAdapter([NotNull] Context context, int textViewResourceId, [NotNull] IList<T> objects, int selectedPosition) : base(context, textViewResourceId, objects)
        {
            SelectedPosition = selectedPosition;
            SetDropDownViewResource(Resource.Layout.simple_spinner_dropdown_item);
        }
    }
}