using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        private List<Enemy1> enemies1;
        private List<Enemy2> enemies2;

       

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
                LocationX = CanvasWidth/2,
                LocationY = 600
            
            };
            // tähän vielä children tavalla pelaajan lisäys peliin
            MyCanvas.Children.Add(player);

            CreateEnemies1();

            player.UpdateLocation();

            // game loop
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Start();
        }

        private void CreateEnemies1()
        {
            enemies1 = new List<Enemy1>();
            int enemy1Count = 20;
            int cols = 4;
            int xStartPos = 55;
            int yStartPos = 50;
            int step = 5;
            int row = 0;
            int col = 0;

            for (int i = 0; i < enemy1Count; i++)
            {
                if (i % cols == 0 && i > 0)
                {
                    row++;
                    col = 0;
                }
                else if (i > 0)
                {
                    col++;
                }
                int x = (55 + step) * col + xStartPos;
                int y = (105 + step) * col + yStartPos;

                Enemy1 enemy1 = new Enemy1
                {
                    LocationX = x,
                    LocationY = y
                };
                enemies1.Add(enemy1);
                MyCanvas.Children.Add(enemy1);
                enemy1.SetLocation();
            }
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
                    SpacePressed = false;
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
