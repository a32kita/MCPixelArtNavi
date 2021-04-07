﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MCPArtNavi.UserApp.PixelCanvasInternal
{
    public class PixelCanvasMapHandler
    {
        // 非公開フィールド
        private EventHandler<PixelEventArgs> _getPixelRequested;
        private EventHandler<PixelEventArgs> _setPixelRequested;
        private EventHandler _redrawLayoutRequested;
        private EventHandler<CanvasToBitmapEventArgs> _canvasToBitmapRequested;
        private EventHandler<InvokeDispatcherEventArgs> _invokeDispatcherRequested;


        // 公開イベント

        public event EventHandler<PixelEventArgs> GetPixelRequested
        {
            add => this._getPixelRequested += value;
            remove => this._getPixelRequested -= value;
        }

        public event EventHandler<PixelEventArgs> SetPixelRequested
        {
            add => this._setPixelRequested += value;
            remove => this._setPixelRequested -= value;
        }

        public event EventHandler RedrawLayoutRequested
        {
            add => this._redrawLayoutRequested += value;
            remove => this._redrawLayoutRequested -= value;
        }

        public event EventHandler<CanvasToBitmapEventArgs> CanvasToBitmapRequested
        {
            add => this._canvasToBitmapRequested += value;
            remove => this._canvasToBitmapRequested -= value;
        }

        public event EventHandler<InvokeDispatcherEventArgs> InvokeDispatcherRequested
        {
            add => this._invokeDispatcherRequested += value;
            remove => this._invokeDispatcherRequested -= value;
        }



        // 公開メソッド

        public Brush GetPixel(int x, int y)
        {
            try
            {
                var eventArgs = new PixelEventArgs() { X = x, Y = y };
                this._getPixelRequested?.Invoke(this, eventArgs);
                return eventArgs.Brush;
            }
            catch
            {
                // NOP
            }

            return null;
        }

        public bool SetPixel(int x, int y, Brush brush)
        {
            try
            {
                this._setPixelRequested?.Invoke(this, new PixelEventArgs()
                {
                    X = x,
                    Y = y,
                    Brush = brush
                });
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void RedrawLayout()
        {
            this._redrawLayoutRequested?.Invoke(this, new EventArgs());
        }

        public BitmapSource CanvasToBitmap()
        {
            try
            {
                var eventArgs = new CanvasToBitmapEventArgs();
                this._canvasToBitmapRequested?.Invoke(this, eventArgs);
                return eventArgs.Bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.GetType().Name}: ${ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return null;
        }

        public void InvokeDispatcher(Action action)
        {
            this._invokeDispatcherRequested?.Invoke(this, new InvokeDispatcherEventArgs() { Action = action });
        }

        
        // その他

        public class PixelEventArgs
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Brush Brush { get; set; }
        }

        public class CanvasToBitmapEventArgs
        {
            public BitmapSource Bitmap
            {
                get;
                set;
            }
        }

        public class InvokeDispatcherEventArgs
        {
            public Action Action
            {
                get;
                set;
            }
        }
    }
}
