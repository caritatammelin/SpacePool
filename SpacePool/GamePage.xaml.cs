using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpacePool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private Player player;
        private DispatcherTimer timer;
        
        // location
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        // canvas width and height (used to randomize a new flower)
        private double CanvasWidth;
        private double CanvasHeight;

        //keys
        private bool SpacePressed;
        private bool LeftPressed;
        private bool RightPressed;
        private bool UpPressed;

        List<Bullet> bullets = new List<Bullet>();

        public GamePage()
        {
            this.InitializeComponent();

            // change the default startup mode
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // specify the size
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);


            // key listeners
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            // get canvas width and height
            CanvasWidth = MyCanvas.Width;
            CanvasHeight = MyCanvas.Height;

            // add player
            player = new Player
            {
                LocationX = CanvasWidth / 2,
                LocationY = 600

            };



            MyCanvas.Children.Add(player);

            player.UpdateLocation();


            // game loop
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Start();

            

        }
        
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    LeftPressed = false;
                    Debug.WriteLine("Left Pressed");
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
                case VirtualKey.Space:
                    SpacePressed = false;
                    //Debug.WriteLine("Space");
                    break;
                default:
                    break;
            }
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;
                case VirtualKey.Space:
                    SpacePressed = true;
                    break;
                case VirtualKey.Up:
                    UpPressed = false;
                    break;
                default:
                    break;
            }
        }

        private void Timer_Tick(object sender, object e)
        {

            if (SpacePressed)
            {
                Bullet bullet = new Bullet
                {
                    LocationX = player.LocationX,
                    LocationY = player.LocationY
                };
                MyCanvas.Children.Add(bullet);
                bullets.Add(bullet);

                SpacePressed = false;
            }
            foreach (Bullet bullet in bullets)
            {
                bullet.Move();
                bullet.UpdateLocation();

                if (bullet.LocationY < 0)
                {
                    bullets.Remove(bullet);
                    break;
                }

            }


            if (LeftPressed) player.Move(1);
            if (RightPressed) player.Move(-1);
        }

        private void CheckCollision()
        {

        }

    }
}