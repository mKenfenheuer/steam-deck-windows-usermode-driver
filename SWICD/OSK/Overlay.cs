using GameOverlay.Drawing;
using GameOverlay.Windows;
using neptune_hidapi.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWICD.OSK
{
    /// <summary>
    /// Overlay For Touchpad Aim circle(2 red dot)
    /// Added thirdparty library named GameOverlay. could use sharp2dx directly, but i thought this would be a more efficient way..
    /// </summary>
    internal class Overlay : IDisposable
    {
        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int LWA_ALPHA = 0x2;

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll")]
        static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static double Map(double a1, double a2, double b1, double b2, double s) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);


        IntPtr _oskHandle = IntPtr.Zero;

        Graphics _gfx;
        private GraphicsWindow _window;
        private Dictionary<string, SolidBrush> _brushes;
        private Dictionary<string, Font> _fonts;
        private Dictionary<string, Image> _images;

        private float _leftXPos, _leftYPos, _rightXPos, _rightYPos;
        private bool _leftTouched = false;
        private bool _rightTouched = false;
        private bool _leftPressed = false;
        private bool _rightPressed = false;
        private PointerTouchInfo _leftContact;
        private PointerTouchInfo _rightContact;

        private int _overlabPercentage = 55;
        private byte _oskTransparent = 127;
        private int _offsetY = 0;

        public Queue<PointerTouchInfo> EventTouch { get => _eventTouch; set => _eventTouch = value; }
        public int OverlabPercentage { get => _overlabPercentage; set => _overlabPercentage = value; }
        public byte OskTransparent { get => _oskTransparent; set => _oskTransparent = value; }
        public int OffsetY { get => _offsetY; set => _offsetY = value; }

        Queue<PointerTouchInfo> _eventTouch = new Queue<PointerTouchInfo>();

        public Overlay()
        {
            InitializeComponent();

            _brushes = new Dictionary<string, SolidBrush>();
            _fonts = new Dictionary<string, Font>();
            _images = new Dictionary<string, Image>();

            _gfx = new Graphics()
            {
                MeasureFPS = false,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };

            _window = new GraphicsWindow(0, 0, 1280, 600, _gfx)
            {
                FPS = 60,
                IsTopmost = true,
                IsVisible = true
            };

            _window.DestroyGraphics += _window_DestroyGraphics;
            _window.DrawGraphics += _window_DrawGraphics;
            _window.SetupGraphics += _window_SetupGraphics;
        }

        public void Show()
        {
            if (_window.IsInitialized)
                _window.Show();
        }

        public void Hide()
        {
            if (_window.IsInitialized)
                _window.Hide();
        }


        private void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
        {
            foreach (var pair in _brushes) pair.Value.Dispose();
            foreach (var pair in _fonts) pair.Value.Dispose();
            foreach (var pair in _images) pair.Value.Dispose();
        }

        public void SetLeftPadPos(float leftX, float leftY)
        {
            if (leftX == 0 && leftY == 0)
            {
                _leftTouched = false;
                return;
            }
                

            float mapped_leftX = (float)Map(short.MinValue, short.MaxValue, 0, 1, leftX);
            float mapped_leftY = 1f - (float)Map(short.MinValue, short.MaxValue, 0, 1, leftY);

            _leftXPos = _window.Width * 0.01f * OverlabPercentage * mapped_leftX;
            _leftYPos = _window.Height * mapped_leftY + OffsetY;
            _leftTouched = true;
        }
        public void SetRightPadPos(float rightX, float rightY)
        {
            if(rightX == 0 && rightY == 0)
            {
                _rightTouched = false;
                return;
            }

            float mapped_rightX = (float)Map(short.MinValue, short.MaxValue, 0, 1, rightX);
            float mapped_rightY = 1f - (float)Map(short.MinValue, short.MaxValue, 0, 1, rightY);

            _rightXPos = (_window.Width - _window.Width * 0.01f * OverlabPercentage) + _window.Width * 0.01f * OverlabPercentage * mapped_rightX;
            _rightYPos = _window.Height * mapped_rightY + OffsetY;
            _rightTouched = true;
        }


        private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            gfx.ClearScene(_brushes["background"]);

            if (_leftTouched)
            {
                gfx.DrawCircle(_brushes["touch"], _leftXPos, _leftYPos, 10, 5.0f);
            }

            if (_rightTouched)
            {
                gfx.DrawCircle(_brushes["touch"], _rightXPos, _rightYPos, 10, 5.0f);
            }
        }

        private void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            if (e.RecreateResources)
            {
                foreach (var pair in _brushes) pair.Value.Dispose();
                foreach (var pair in _images) pair.Value.Dispose();
            }

            _brushes["touch"] = gfx.CreateSolidBrush(255, 0, 0);
            _brushes["background"] = gfx.CreateSolidBrush(255, 255, 255, 0);

            if (e.RecreateResources) return;
        }

        public bool Init()
        {
            _window.Create();

            _oskHandle = FindWindow("ApplicationFrameWindow", "");

            bool isValidHandle = (IntPtr.Size == 4) ? (_oskHandle.ToInt32() > 0) : (_oskHandle.ToInt64() > 0);
            if (isValidHandle)
            {
                SetWindowLong(_oskHandle, GWL_EXSTYLE, GetWindowLong(_oskHandle, GWL_EXSTYLE) | WS_EX_LAYERED);
                SetLayeredWindowAttributes(_oskHandle, 0, _oskTransparent, LWA_ALPHA);
                return true;
            }

            return false;
        }

        public void AddEventTouch(int xPos, int yPos)
        {
            Task.Factory.StartNew(() =>
            {
                PointerTouchInfo touchInfo = MakePointerTouchInfo(xPos, yPos, 5, 0);
                touchInfo.PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
                TouchInjector.InjectTouchInput(1, new PointerTouchInfo[1] { touchInfo });

                Thread.Sleep(100);
                touchInfo.PointerInfo.PointerFlags = PointerFlags.UP;
                TouchInjector.InjectTouchInput(1, new PointerTouchInfo[1] { touchInfo });
            });
        }

        public void ProcessTouch(bool left, bool right)
        {
            if (_leftPressed == false && left)
            {
                _leftPressed = true;
                _leftContact = MakePointerTouchInfo((int)(_window.X + _leftXPos), (int)(_window.Y + _leftYPos), 5, 0);
                PointerFlags oFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
                _leftContact.PointerInfo.PointerFlags = oFlags;
                EventTouch.Enqueue(_leftContact);
            }
            else if (left)
            {
                _leftContact.Move((int)(_window.X + _leftXPos - _leftContact.PointerInfo.PtPixelLocation.X), (int)(_window.Y + _leftYPos - _leftContact.PointerInfo.PtPixelLocation.Y));
                PointerFlags oFlags = PointerFlags.INRANGE | PointerFlags.INCONTACT | PointerFlags.UPDATE;
                _leftContact.PointerInfo.PointerFlags = oFlags;
                EventTouch.Enqueue(_leftContact);
            }
            else if (_leftPressed && left == false)
            {
                _leftContact.PointerInfo.PointerFlags = PointerFlags.UP;
                _leftPressed = false;
                EventTouch.Enqueue(_leftContact);
            }

            if (_rightPressed == false && right)
            {
                _rightPressed = true;
                _rightContact = MakePointerTouchInfo((int)(_window.X + _rightXPos), (int)(_window.Y + _rightYPos), 5, 0);
                PointerFlags oFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
                _rightContact.PointerInfo.PointerFlags = oFlags;
                EventTouch.Enqueue(_rightContact);
            }
            else if (right)
            {
                _rightContact.Move((int)(_window.X + _rightXPos - _rightContact.PointerInfo.PtPixelLocation.X), (int)(_window.Y + _rightYPos - _rightContact.PointerInfo.PtPixelLocation.Y));
                PointerFlags oFlags = PointerFlags.INRANGE | PointerFlags.INCONTACT | PointerFlags.UPDATE;
                _rightContact.PointerInfo.PointerFlags = oFlags;
                EventTouch.Enqueue(_rightContact);
            }
            else if (_rightPressed && right == false)
            {
                _rightContact.PointerInfo.PointerFlags = PointerFlags.UP;
                _rightPressed = false;
                EventTouch.Enqueue(_rightContact);
            }


            TouchInjector.InjectTouchInput(EventTouch.Count, EventTouch.ToArray());
            EventTouch.Clear();
        }

        private PointerTouchInfo MakePointerTouchInfo(int x, int y, int radius,
            uint orientation = 90, uint pressure = 32000)
        {
            PointerTouchInfo contact = new PointerTouchInfo();
            contact.PointerInfo.pointerType = PointerInputType.TOUCH;
            contact.TouchFlags = TouchFlags.NONE;
            contact.Orientation = orientation;
            contact.Pressure = pressure;
            contact.TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE;
            contact.PointerInfo.PtPixelLocation.X = x;
            contact.PointerInfo.PtPixelLocation.Y = y;
            uint unPointerId = IdGenerator.GetUinqueUInt();
            contact.PointerInfo.PointerId = unPointerId;
            contact.ContactArea.left = x - radius;
            contact.ContactArea.right = x + radius;
            contact.ContactArea.top = y - radius;
            contact.ContactArea.bottom = y + radius;
            return contact;
        }

        public void SetRect(int xPos, int yPos, int width, int height)
        {
            if (height <= 0)
                height = 1;
            if (width <= 0)
                width = 1;
            _window.Resize(xPos, yPos, width, height);
        }


        private void InitializeComponent()
        {
        }

        public void Dispose()
        {
            _window.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
