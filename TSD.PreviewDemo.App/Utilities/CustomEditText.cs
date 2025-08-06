using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
// ReSharper disable IdentifierTypo
#pragma warning disable CS0169 // Field is never used

namespace TSD.PreviewDemo.App.Utilities
{
    public sealed class CustomEditText : EditText
    {
        public InputMethodManager Manager { get; set; }

        private InputTypes _sourceInputType;
        public CustomEditText(Context context) : base(context)
        {
            InputType = InputTypes.Null;
        }

        public CustomEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            

        }

        public CustomEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            
        }

        public CustomEditText(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        { }

        public override bool DispatchTouchEvent(MotionEvent e)
        {              
            var result = base.DispatchTouchEvent(e);
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            return result;
        }

        public override void OnEditorAction([GeneratedEnum] ImeAction actionCode)
        {            
            var prewInputType = InputType;
            InputType = InputTypes.Null;
            base.OnEditorAction(actionCode);
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            InputType = prewInputType;
        }       

        protected override void OnFocusChanged(bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Rect previouslyFocusedRect)
        {
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
        }

        public override void SelectAll()
        {
            var prewInputType = InputType;
            InputType = InputTypes.Null;
            base.SelectAll();
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            InputType = prewInputType;
        }

       public override void SetSelection(int index)
        {
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            base.SetSelection(index);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
        }

       public override void ExtendSelection(int index)
        {
            var prewInputType = InputType;
            InputType = InputTypes.Null;
            base.ExtendSelection(index);
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            InputType = prewInputType;
        }

       public override void SetSelection(int start, int stop)
        {
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            base.SetSelection(start, stop);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            var result = base.OnTouchEvent(e);
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            return result;
        }
        public override bool OnKeyLongPress([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var prewInputType = InputType;
            InputType = InputTypes.Null;
            var result = base.OnKeyLongPress(keyCode, e);
            var imm = (InputMethodManager)Context?.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(WindowToken, 0);
            InputType = prewInputType;
            return result;
        }
    }
}