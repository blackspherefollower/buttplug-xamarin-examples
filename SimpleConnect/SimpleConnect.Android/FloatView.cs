using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SimpleConnect.Droid
{
    public class FloatView : ImageView, View.IOnTouchListener
    {
        private int xInitCord = 0;
        private int yInitCord = 0;
        private int xInitMargin = 0;
        private int yInitMargin = 0;
        private IWindowManager windowManager;
        private WindowManagerLayoutParams layoutParams;
        private DateTime? longPress = null;

        public event EventHandler OnClicked;

        public FloatView(Context context) : base(context)
        {
            Init(context);
        }

        public FloatView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public FloatView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context);
        }

        public FloatView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs,
            defStyleAttr, defStyleRes)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            layoutParams = new WindowManagerLayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent,
                WindowManagerTypes.ApplicationOverlay,
                WindowManagerFlags.NotFocusable,
                Format.Translucent);

            layoutParams.Gravity = GravityFlags.Top | GravityFlags.Left;
            windowManager.AddView(this, layoutParams);

            SetOnTouchListener(this);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            int xCord = Convert.ToInt32(e.RawX);
            int yCord = Convert.ToInt32(e.RawY);

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    xInitCord = xCord;
                    yInitCord = yCord;
                    xInitMargin = layoutParams.X;
                    yInitMargin = layoutParams.Y;
                    longPress = DateTime.Now.AddSeconds(1);
                    break;

                case MotionEventActions.Move:
                    if (longPress == null || longPress > DateTime.Now)
                        break;
                    int xDiffMove = xCord - xInitCord;
                    int yDiffMove = yCord - yInitCord;
                    int xCordDestination = xInitMargin + xDiffMove;
                    int yCordDestination = yInitMargin + yDiffMove;

                    layoutParams.X = xCordDestination;
                    layoutParams.Y = yCordDestination;
                    windowManager.UpdateViewLayout(this, layoutParams);
                    break;

                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    if(longPress != null && longPress > DateTime.Now)
                        OnClicked?.Invoke(this, EventArgs.Empty);
                    longPress = null;
                    break;

            }

            return true;
        }
    }
}