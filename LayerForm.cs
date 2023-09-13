
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;


namespace ExplorerShade
{
    public partial class LayerForm : Form
    {
        private Dictionary<IntPtr, Bitmap> explorerBackgrounds = new Dictionary<IntPtr, Bitmap>();
        private Dictionary<IntPtr, Rectangle> explorerWindowRects = new Dictionary<IntPtr, Rectangle>();
        private bool explorerWindowFocused = false;
        private IntPtr lastFocusedWindow = IntPtr.Zero;



        public LayerForm()
        {
            InitializeComponent();

            // Set the form properties to achieve the desired appearance
            this.FormBorderStyle = FormBorderStyle.None; // No border
            this.ShowInTaskbar = false; // No taskbar button
            this.TransparencyKey = Color.CadetBlue; // Set a color that should be treated as transparent
            this.Opacity = 0.6; // St the opacity to 60% (adjust as needed)
            this.ControlBox = false; // removes the titlebar

            //this.Deactivate += LayerForm_Deactivate; // Add a Deactivate event handler
            //this.LostFocus += LayerForm_LostFocus; // Add a LostFocus event handler


            InitializeExplorerShade();
        }

        private void CheckFocusedWindowAndActivateLayer()
        {
            IntPtr focusedWindow = GetForegroundWindow();

            // Check if the focused window is a File Explorer window
            string className = GetWindowClassName(focusedWindow);
            if (className == "CabinetWClass") // Example class name for File Explorer
            {
                // Capture the ID of the focused window
                lastFocusedWindow = focusedWindow;

                // Activate the layer when the focused window is a File Explorer window
                this.Show();
                this.TopMost = false; // Ensure it's not the topmost window
                SetWindowPos(this.Handle, focusedWindow, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
            else
            {
                // Deactivate the layer when no File Explorer window is focused
                this.Hide();
            }
        }

        private void UpdateLayerAndRestoreFocus()
        {
            // Check the focused window and activate/deactivate the layer
            CheckFocusedWindowAndActivateLayer();

            // Do the layer update, e.g., refresh the layer
            // Add your code to update the layer here

            // Restore focus to the last focused window
            IntPtr currentForegroundWindow = GetForegroundWindow();
            if (currentForegroundWindow == this.Handle)
            {
                SetForegroundWindow(lastFocusedWindow);
            }
        }


        private void InitializeExplorerShade()
        {
            // Start a timer to continuously monitor and update File Explorer windows
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10; // Adjust the interval as needed, in ms
            timer.Tick += new EventHandler(UpdateExplorerWindows);
            timer.Start();
            //this.Activated += LayerForm_Activated; // Add an Activated event handler
        }

        private void UpdateExplorerWindows(object sender, EventArgs e)
        {
            // Enumerate all top-level windows
            EnumWindowsProc callback = new EnumWindowsProc(EnumWindowsCallback);
            EnumWindows(callback, 0);
            UpdateLayerAndRestoreFocus();
        }
        private string GetWindowClassName(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder className = new StringBuilder(nChars);
            if (GetClassName(hWnd, className, nChars) != 0)
            {
                return className.ToString();
            }
            return string.Empty;
        }

        private Bitmap CreateBackgroundImage(IntPtr hWnd)
        {
            // You need to implement this function based on your requirements.
            // Here's a simple example to create a red background image:

            int width = this.Width; // Width of LayerForm
            int height = this.Height; // Height of LayerForm

            Bitmap background = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(background))
            {
                // Fill the background with a red color (adjust as needed)
                g.Clear(Color.FromArgb(38, 0, 0, 0)); // 38 represents 15% opacity
            }

            return background;
        }

        private bool EnumWindowsCallback(IntPtr hWnd, int lParam)
        {
            // Check if the window is a File Explorer window based on criteria like class name
            string className = GetWindowClassName(hWnd);
            if (className == "CabinetWClass") // Example class name for File Explorer
            {
                // Check if the window is already associated with a background
                if (!explorerBackgrounds.ContainsKey(hWnd))
                {
                    // Create a new background image for the File Explorer window
                    Bitmap background = CreateBackgroundImage(hWnd);
                    explorerBackgrounds[hWnd] = background;
                }

                // Get window position
                RECT rect;
                GetWindowRect(hWnd, out rect);

                // Calculate the width and height of the window
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                // Create a System.Drawing.Rectangle to store the window position and size
                Rectangle windowRect = new Rectangle(rect.Left + 10, rect.Top, width - 20, height - 10);

                // Check if the window has moved or changed size
                if (!explorerWindowRects.ContainsKey(hWnd) ||
                    explorerWindowRects[hWnd] != windowRect) // Compare with stored values
                {
                    // Update the stored window position and size
                    explorerWindowRects[hWnd] = windowRect;

                    // Position and size the LayerForm to match the File Explorer window
                    this.Bounds = windowRect;

                    // Render and display the background image on the LayerForm
                    this.BackgroundImage = explorerBackgrounds[hWnd];
                    this.Invalidate(); // Signal that the form's graphics need to be refreshed, but don't bring it to the foreground
                }
            }

            return true;
        }

        // Implement GetWindowClassName, CreateBackgroundImage, and other necessary functions here

        // Windows API functions and structures
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]

        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, int lParam);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Rest of your form initialization code
    }
}
